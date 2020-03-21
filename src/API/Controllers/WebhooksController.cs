using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tayra.Common;
using Tayra.Connectors.Atlassian;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WebhooksController : BaseController
    {
        #region Constructor

        public WebhooksController(IServiceProvider serviceProvider, OrganizationDbContext dbContext) : base(serviceProvider)
        {
            DbContext = dbContext;
        }

        #endregion

        #region Properties

        private OrganizationDbContext DbContext { get; }

        #endregion

        #region Action Methods

        private void SaveWebhookEventLog(JObject jObject)
        {
            DbContext.WebhookEventLogs.Add(new WebhookEventLog { Data = jObject.ToString(Formatting.None) });
            DbContext.SaveChanges();
        }

        [HttpPost("atjissueupdate")]
        [AllowAnonymous]
        public ActionResult JiraIssueUpdate([FromBody] JObject jObject)
        {
            SaveWebhookEventLog(jObject);
            WebhookEvent we = jObject.ToObject<WebhookEvent>();

            var fields = we.JiraIssue.Fields;
            var jiraProjectId = fields.Project.Id;

            fields.Timespent = fields.Timespent.HasValue ? fields.Timespent / 60 : null;
            fields.TimeOriginalEstimate = fields.TimeOriginalEstimate.HasValue ? fields.TimeOriginalEstimate / 60 : null;

            var rewardStatusField = DbContext
                                .IntegrationFields
                                .LastOrDefault(x => x.Key == ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_ + jiraProjectId);

            if (rewardStatusField == null)
            {
                throw new ApplicationException($"Jira project with Id: {jiraProjectId} is not connected to any tayra segments");
            }

            var jiraConnector = new AtlassianJiraConnector(null, DbContext);
            var statusChangelogs = jiraConnector.GetIssueChangelog(rewardStatusField.IntegrationId, we.JiraIssue.Key, "status");

            if (statusChangelogs.Last().Created.ToUniversalTime() != DateTimeExtensions.ConvertUnixEpochTime(we.Timestamp))
            {
                return Ok();
            }

            var assigneProfile = fields?.Assignee == null ? null : ProfilesService.GetMemberByExternalId(fields.Assignee.AccountId, IntegrationType.ATJ);
            var profileAssignment = assigneProfile == null ? null : DbContext.ProfileAssignments.FirstOrDefault(x => x.ProfileId == assigneProfile.Id); //TODO: we need to append segmentId to webhooks
            var currentSegmentId = profileAssignment != null ? profileAssignment.SegmentId : (int?)null;

            var activeCompetitions = assigneProfile == null ? null : CompetitionsService.GetActiveCompetitions(assigneProfile.Id);
            var jiraBaseUrl = we.JiraIssue.Self.Substring(0, we.JiraIssue.Self.IndexOf('/', 10)); //TODO: is 10 ok for all integration types?
            if (assigneProfile == null || fields.Status.Id != rewardStatusField.Value)
            {
                TasksService.AddOrUpdate(new TaskAddOrUpdateDTO
                {
                    ExternalId = we.JiraIssue.Key,
                    ExternalProjectId = fields.Project.Id,
                    IntegrationType = IntegrationType.ATJ,
                    Summary = fields.Summary,
                    JiraStatusCategory = fields.Status.Category.Id,
                    TimeSpentInMinutes = fields.Timespent,
                    TimeOriginalEstimatInMinutes = fields.TimeOriginalEstimate,
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
                    LogsService.LogEvent(new LogCreateDTO
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
                        ProfileId = assigneProfile.Id,
                        CompetitionIds = activeCompetitions.Select(x => x.Id).Distinct()
                    });
                }

                DbContext.SaveChanges();
                return Ok();
            }

            //if we came here assigneeProfile is not null AND Status is set to RewardStatus 
            int? autoTimeSpent = null;
            fields.Timespent = fields.Timespent > 0 ? fields.Timespent : null; //redundant, check above
            {
                var statuses = jiraConnector.GetProjectStatuses(rewardStatusField.IntegrationId, jiraProjectId, fields.IssueType.Id);
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

            var timeSpentToUse = fields.Timespent ?? autoTimeSpent / 3;
            var effortScore = TayraEffortCalculator.CalcEffortScore(timeSpentToUse ?? 0, TayraPersonalPerformance.MapSPToComplexity((int?)fields.StoryPointsCF ?? 0));

            TokensService.CreateTransaction(TokenType.CompanyToken, assigneProfile.Id, effortScore, TransactionReason.JiraIssueCompleted, ClaimBundleTypes.EarnedFromWork);
            TokensService.CreateTransaction(TokenType.Experience, assigneProfile.Id, effortScore, TransactionReason.JiraIssueCompleted, ClaimBundleTypes.EarnedFromWork);

            TasksService.AddOrUpdate(new TaskAddOrUpdateDTO
            {
                ExternalId = we.JiraIssue.Key,
                IntegrationType = IntegrationType.ATJ,
                Summary = fields.Summary,
                JiraStatusCategory = fields.Status.Category.Id,
                AutoTimeSpentInMinutes = autoTimeSpent,
                TimeSpentInMinutes = fields.Timespent,
                TimeOriginalEstimatInMinutes = fields.TimeOriginalEstimate,
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

            LogsService.LogEvent(new LogCreateDTO
            {
                Event = LogEvents.StatusChangeToCompleted,
                Data = new Dictionary<string, string>
                {
                    { "timestamp", DateTime.UtcNow.ToString() },
                    { "issueUrl", jiraBaseUrl + "/browse/" + we.JiraIssue.Key },
                    { "issueKey", we.JiraIssue.Key },
                    { "issueSummary", fields.Summary },
                    { "issueStatus", fields.Status.Name },
                    { "effortScore", Math.Round(effortScore, 2).ToString() },
                    { "profileUsername", assigneProfile.Username },
                    { "competitorName", activeCompetitions.FirstOrDefault()?.CompetitorName},
                    { "timespent", timeSpentToUse.ToString()}
                },
                ProfileId = assigneProfile.Id,
                CompetitionIds = activeCompetitions.Select(x => x.Id).Distinct()
            });

            var aps = DbContext.ActionPoints.Where(x => x.ProfileId == assigneProfile.Id && x.ConcludedOn == null && (x.Type == ActionPointTypes.ProfilesNoCompletedTasksIn1Week || x.Type == ActionPointTypes.ProfilesNoCompletedTasksIn2Week)).ToArray();
            foreach (var ap in aps)
            {
                ap.ConcludedOn = DateTime.UtcNow;
            }

            DbContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}