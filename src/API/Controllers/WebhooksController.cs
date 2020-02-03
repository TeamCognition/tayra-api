using System;
using System.Collections.Generic;
using System.Linq;
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
                throw new ApplicationException($"issue is located in {jiraProjectId} jira segment which is not connected to any tayra segments");
            }

            var jiraConnector = new AtlassianJiraConnector(null, DbContext);
            var statusChangelogs = jiraConnector.GetIssueChangelog(rewardStatusField.IntegrationId, we.JiraIssue.Key, "status");

            if (statusChangelogs.Last().Created.ToUniversalTime() != ConvertUnixEpochTime(we.Timestamp))
            {
                return Ok();
            }

            var assigneProfile = ProfilesService.GetByExternalId(fields.Assignee.AccountId, IntegrationType.ATJ);
            var teamMember = DbContext.TeamMembers.FirstOrDefault(x => x.ProfileId == assigneProfile.Id);
            var currentSegmentId = teamMember != null ? DbContext.Teams.Where(x => x.Id == teamMember.TeamId).Select(x => x.SegmentId).FirstOrDefault() : (int?)null;

            var activeCompetitions = CompetitionsService.GetActiveCompetitions(assigneProfile.Id);
            var jiraBaseUrl = we.JiraIssue.Self.Substring(0, we.JiraIssue.Self.IndexOf('/', 10));
            if (fields.Status.Id != rewardStatusField.Value)
            {
                TasksService.AddOrUpdate(new TaskAddOrUpdateDTO
                {
                    ExternalId = we.JiraIssue.Key,
                    IntegrationType = IntegrationType.ATJ,
                    Summary = fields.Summary,
                    JiraStatusCategory = fields.Status.Category.Id,
                    TimeSpentInMinutes = fields.Timespent,
                    TimeOriginalEstimatInMinutes = fields.TimeOriginalEstimate,
                    StoryPoints = fields.StoryPointsCF,
                    Priority = GetTaskPriority(fields.Priority.Id),
                    Type = GetTaskType(fields.IssueType.Id),
                    EffortScore = null,
                    Labels = fields.Labels,
                    AssigneeProfileId = assigneProfile.Id,
                    ReporterProfileId = 0,
                    TeamId = teamMember?.TeamId,
                    SegmentId = currentSegmentId
                });

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

                DbContext.SaveChanges();
                return Ok();
            }

            int? autoTimeSpent = null;
            fields.Timespent = fields.Timespent > 0 ? fields.Timespent : null; //redundant, check above
            if (fields.Timespent == null)
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

            var timeSpentToUse = fields.Timespent ?? autoTimeSpent;
            var effortScore = TayraEffortCalculator.CalcEffortScore(timeSpentToUse ?? 0, TayraPersonalPerformance.MapSPToComplexity(fields.StoryPointsCF ?? 0));

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
                StoryPoints = fields.StoryPointsCF,
                Priority = GetTaskPriority(fields.Priority.Id),
                Type = GetTaskType(fields.IssueType.Id),
                EffortScore = effortScore,
                Labels = fields.Labels,
                AssigneeProfileId = assigneProfile.Id,
                ReporterProfileId = 0,
                TeamId = teamMember?.TeamId,
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

            DbContext.SaveChanges();

            return Ok();
        }

        private DateTime ConvertUnixEpochTime(long milliseconds)
        {
            DateTime Fecha = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Fecha.AddMilliseconds(milliseconds);
        }


        private TaskPriorities GetTaskPriority(string jiraIssuePriorityId)
        {
            switch (jiraIssuePriorityId)
            {
                case "1":
                    return TaskPriorities.Highest;

                case "2":
                    return TaskPriorities.High;

                case "3":
                    return TaskPriorities.Medium;

                case "4":
                    return TaskPriorities.Low;

                case "5":
                    return TaskPriorities.Lowest;

                default: return TaskPriorities.Medium;
            }
        }

        private TaskTypes GetTaskType(string jiraIssueTypeId)
        {
            switch (jiraIssueTypeId)
            {
                case "10006":
                    return TaskTypes.Bug;

                default: return TaskTypes.Task;
            }
        }

        #endregion
    }
}