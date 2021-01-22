using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tayra.Services.Models.Profiles;
using Tayra.Common;
using Tayra.Connectors.Atlassian;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices
{
    public class ATJIssueUpdateLoader : BaseLoader
    {
        #region Constructor

        public ATJIssueUpdateLoader(LogService logService, CatalogDbContext catalogDb, IConfiguration config) : base(logService, catalogDb)
        {
            Config = config;
        }

        #endregion

        protected IConfiguration Config;
        
        #region Public Methods

        public override void Execute(DateTime date, JObject requestBody, params Tenant[] tenants)
        {
            foreach (var tenant in tenants)
            {
                LogService.SetOrganizationId(tenant.Identifier);
                using (var organizationDb = new OrganizationDbContext(TenantModel.WithConnectionStringOnly(tenant.ConnectionString), null))
                {
                    IssueUpdate(organizationDb, date, LogService, requestBody, Config);
                }
            }
        }

        private static void SaveWebhookEventLog(OrganizationDbContext organizationDb, JObject jObject)
        {
            organizationDb.WebhookEventLogs.Add(new WebhookEventLog { Data = jObject.ToString(Formatting.None) });
            organizationDb.SaveChanges();
        }

        public static void IssueUpdate(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService, JObject requestBody, IConfiguration config)
        {
            SaveWebhookEventLog(organizationDb, requestBody);
            JiraWebhookEvent we = requestBody.ToObject<JiraWebhookEvent>();

            var fields = we.JiraIssue.Fields;
            var jiraProjectId = fields.Project.Id;

            fields.Timespent = fields.Timespent.HasValue ? fields.Timespent / 60 : null;
            fields.TimeOriginalEstimate = fields.TimeOriginalEstimate.HasValue ? fields.TimeOriginalEstimate / 60 : null;

            var rewardStatusField = organizationDb
                                .IntegrationFields
                                .LastOrDefault(x => x.Key == ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_ + jiraProjectId);

            if (rewardStatusField == null)
            {
                throw new ApplicationException($"Jira project with Id: {jiraProjectId} is not connected to any tayra segments");
            }

            var jiraSiteName = organizationDb
                            .IntegrationFields
                            .LastOrDefault(x => x.Key == ATConstants.AT_SITE_NAME)?.Value;

            var jiraConnector = new AtlassianJiraConnector(null, organizationDb, null, config);
            var statusChangelogs = jiraConnector.GetIssueChangelog(rewardStatusField.IntegrationId, we.JiraIssue.Key, "status");

            if (statusChangelogs.Last().Created.ToUniversalTime() != DateTimeExtensions.ConvertUnixEpochTime(we.Timestamp))
            {
                return;
            }

            var tasksService = new TasksService(organizationDb);
            
            var logsService = new LogsService(organizationDb);
            var tokensService = new TokensService(organizationDb);

            var assigneProfile = fields?.Assignee == null ? null : new ProfilesService().GetProfileByExternalId(organizationDb, fields.Assignee.AccountId, IntegrationType.ATJ);
            var profileAssignment = assigneProfile == null ? null : organizationDb.ProfileAssignments.FirstOrDefault(x => x.ProfileId == assigneProfile.Id); //TODO: we need to append segmentId to webhooks
            var currentSegmentId = profileAssignment != null ? profileAssignment.SegmentId : (Guid?)null;
            
            var jiraBaseUrl = $"https://{jiraSiteName}.atlassian.net";
            if (assigneProfile == null || fields.Status.Id != rewardStatusField.Value)
            {
                tasksService.AddOrUpdate(new TaskAddOrUpdateDTO
                {
                    ExternalId = we.JiraIssue.Key,
                    ExternalProjectId = fields.Project.Id,
                    IntegrationType = IntegrationType.ATJ,
                    Summary = fields.Summary,
                    JiraStatusCategory = fields.Status.Category.Id,
                    TimeSpentInMinutes = fields.Timespent,
                    TimeOriginalEstimateInMinutes = fields.TimeOriginalEstimate,
                    StoryPoints = (int?)fields.StoryPointsCF,
                    Priority = TaskHelpers.GetTaskPriority(fields.Priority.Id),
                    Type = TaskHelpers.GetTaskType(fields.IssueType.Id),
                    EffortScore = null,
                    Labels = fields.Labels,
                    AssigneeExternalId = fields?.Assignee?.AccountId,
                    AssigneeProfileId = assigneProfile?.Id,
                    ReporterProfileId = 0,
                    TeamId = profileAssignment?.TeamId,
                    SegmentId = currentSegmentId
                });

                if (assigneProfile != null)
                {
                    logsService.LogEvent(new LogCreateDTO
                    {
                        Event = LogEvents.IssueStatusChange,
                        Data = new Dictionary<string, string>
                    {
                        { "timestamp", DateTime.UtcNow.ToString() },
                        { "issueUrl", jiraBaseUrl + "/browse/" + we.JiraIssue.Key },
                        { "issueKey", we.JiraIssue.Key },
                        { "issueSummary", fields.Summary },
                        { "issueStatus", fields.Status.Name },
                        { "profileUsername", assigneProfile.Username },
                        { "competitorName", assigneProfile.Username }, //prev: activeCompetitions.FirstOrDefault()?.CompetitorName
                        { "timespent", fields.Timespent.ToString()}
                    },
                        ProfileId = assigneProfile.Id
                    });
                }

                organizationDb.SaveChanges();
                return;
            }

            //if we came here assigneeProfile is not null AND Status is set to RewardStatus
            int? autoTimeSpent = null;
            fields.Timespent = fields.Timespent > 0 ? fields.Timespent : null; //redundant, check above
            {
                var statuses = jiraConnector.GetIssueStatuses(rewardStatusField.IntegrationId, jiraProjectId, fields.IssueType.Id);
                var todoStatuses = statuses.Where(x => x.Category.Id == IssueStatusCategories.ToDo).ToList();
                var inProgressStatuses = statuses.Where(x => x.Category.Id == IssueStatusCategories.InProgress).ToList();
                var doneStatuses = statuses.Where(x => x.Category.Id == IssueStatusCategories.Done).ToList();
                var rewardStatus = rewardStatusField;

                DateTime? enteredInProgress = null;
                DateTime? enteredRewardStatus = null;
                foreach (var cl in statusChangelogs)
                {
                    //if from todo to inProgress
                    if (todoStatuses.Select(x => x.Id).Contains(cl.From) &&
                    inProgressStatuses.Select(x => x.Id).Contains(cl.To))
                    {
                        enteredInProgress = cl.Created;
                    }
                    else if (cl.To == rewardStatus.Value)
                    {
                        enteredRewardStatus = cl.Created;
                    }
                    //back in progress from rewardId
                    else if (cl.From == rewardStatus.Value &&
                    !doneStatuses.Select(x => x.Id).Contains(cl.To))
                    {
                        enteredInProgress = cl.Created;
                        enteredRewardStatus = null;
                    }
                    //back in progress from done
                    else if (doneStatuses.Select(x => x.Id).Contains(cl.From) &&
                    cl.To != rewardStatus.Value.ToString())
                    {
                        enteredInProgress = cl.Created;
                        enteredRewardStatus = null;
                    }
                }

                if (!enteredInProgress.HasValue)
                {
                    if (!enteredRewardStatus.HasValue)
                    {
                        throw new ApplicationException("timespent fallback calculations crashed");
                    }
                    enteredInProgress = enteredRewardStatus;
                }

                var days = (enteredRewardStatus.Value - enteredInProgress.Value).Days;
                var hours = (enteredRewardStatus.Value - enteredInProgress.Value).TotalHours;

                autoTimeSpent = (int)TimeSpan.FromHours((days * 8) + Math.Min(8, hours)).TotalMinutes;
            }

            var effortScore = TayraEffortCalculator.CalcEffortScore(fields.Timespent, autoTimeSpent, TayraPersonalPerformance.MapSPToComplexity((int?)fields.StoryPointsCF ?? 0));

            var task = organizationDb.Tasks.FirstOrDefault(x => x.ExternalId == we.JiraIssue.Key && x.IntegrationType == IntegrationType.ATJ);
            var effortScoreDiff = fields.Timespent == null || task?.EffortScore == null ? effortScore : Math.Max(0, effortScore - task.EffortScore.Value);

            if (effortScoreDiff > 0)
            {
                tokensService.CreateTransaction(TokenType.CompanyToken, assigneProfile.Id, effortScoreDiff, TransactionReason.JiraIssueCompleted, ClaimBundleTypes.EarnedFromWork);
                tokensService.CreateTransaction(TokenType.Experience, assigneProfile.Id, effortScoreDiff, TransactionReason.JiraIssueCompleted, ClaimBundleTypes.EarnedFromWork);
            }

            tasksService.AddOrUpdate(new TaskAddOrUpdateDTO
            {
                ExternalId = we.JiraIssue.Key,
                ExternalProjectId = fields.Project.Id,
                IntegrationType = IntegrationType.ATJ,
                Summary = fields.Summary,
                JiraStatusCategory = fields.Status.Category.Id,
                AutoTimeSpentInMinutes = autoTimeSpent,
                TimeSpentInMinutes = fields.Timespent,
                TimeOriginalEstimateInMinutes = fields.TimeOriginalEstimate,
                StoryPoints = (int?)fields.StoryPointsCF,
                Priority = TaskHelpers.GetTaskPriority(fields.Priority.Id),
                Type = TaskHelpers.GetTaskType(fields.IssueType.Id),
                EffortScore = effortScore,
                Labels = fields.Labels,
                AssigneeExternalId = we.JiraIssue.Fields.Assignee.AccountId,
                AssigneeProfileId = assigneProfile.Id,
                ReporterProfileId = 0,
                TeamId = profileAssignment?.TeamId,
                SegmentId = currentSegmentId
            });

            logsService.LogEvent(new LogCreateDTO
            {
                Event = LogEvents.StatusChangeToCompleted,
                Data = new Dictionary<string, string>
                {
                    { "timestamp", DateTime.UtcNow.ToString() },
                    { "issueUrl", jiraBaseUrl + "/browse/" + we.JiraIssue.Key },
                    { "issueKey", we.JiraIssue.Key },
                    { "issueSummary", fields.Summary },
                    { "issueStatus", fields.Status.Name },
                    { "effortScore", Math.Round(effortScoreDiff, 2).ToString() },
                    { "profileUsername", assigneProfile.Username },
                    { "timespent", TayraEffortCalculator.GetEffectiveTimeSpent(fields.Timespent, autoTimeSpent).ToString()}
                },
                ProfileId = assigneProfile.Id
            });

            var aps = organizationDb.ActionPoints.Where(x => x.ProfileId == assigneProfile.Id && x.ConcludedOn == null && (x.Type == ActionPointTypes.ProfilesNoCompletedTasksIn1Week || x.Type == ActionPointTypes.ProfilesNoCompletedTasksIn2Week)).ToArray();
            foreach (var ap in aps)
            {
                ap.ConcludedOn = DateTime.UtcNow;
            }

            organizationDb.SaveChanges();
        }

        #endregion
    }
}
