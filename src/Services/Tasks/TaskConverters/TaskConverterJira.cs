using System;
using System.Linq;
using Cog.Core;
using Microsoft.Extensions.Configuration;
using Tayra.Common;
using Tayra.Connectors.Atlassian;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Models.Organizations;

namespace Tayra.Services.TaskConverters
{
    public class TaskConverterJira : TaskConverterBase
    {
        protected JiraWebhookEvent We;
        public TaskConverterJira(OrganizationDbContext dbContext,
                                 JiraWebhookEvent we,
                                 IConfiguration config,
                                 TaskConverterMode mode = TaskConverterMode.NORMAL)
                                 : base(dbContext)
        {
            We = we;
            Mode = mode;
            Config = config;
        }

        public TaskConverterJira(OrganizationDbContext dbContext,
                                 JiraIssue jiraIssue,
                                 IConfiguration config,
                                 TaskConverterMode mode = TaskConverterMode.NORMAL)
                                 : base(dbContext)
        {
            We = new JiraWebhookEvent { JiraIssue = jiraIssue };
            Mode = mode;
            Config = config;
        }
        
        public override bool ShouldBeProcessed()
        {
            if (Mode == TaskConverterMode.NORMAL)
            {
                var jiraConnector = new AtlassianJiraConnector(null, DbContext, null, Config);
                var issueChangelogs = jiraConnector.GetIssueChangelog(GetRewardStatus().IntegrationId, GetExternalId(), "status");
                //maybe not needed anymore
                if (issueChangelogs.Last().Created.ToUniversalTime() != DateTimeExtensions.ConvertUnixEpochTime(We.Timestamp))
                {
                    return false;
                }
            }
            return base.ShouldBeProcessed();
        }

        protected override string GetExternalId()
        {
            return We.JiraIssue.Key;
        }

        protected override IntegrationType GetIntegrationType()
        {
            return IntegrationType.ATJ;
        }

        protected override string GetJiraStatusId()
        {
            return We.JiraIssue.Fields.Status.Id;
        }
        
        protected override string GetExternalProjectId()
        {
            return We.JiraIssue.Fields.Project.Id;
        }

        protected override int? GetStoryPoints()
        {
            return (int?)We.JiraIssue.Fields.StoryPointsCF;
        }

        protected override string GetSummary()
        {
            return We.JiraIssue.Fields.Summary;
        }

        protected override int? GetTimeOriginalEstimateInMinutes()
        {
            return We.JiraIssue.Fields.TimeOriginalEstimate / 60;
        }

        protected override string GetAssigneeExternalId()
        {
            return We.JiraIssue.Fields.Assignee?.AccountId;
        }

        protected override string[] GetLabels()
        {
            return We.JiraIssue.Fields.Labels;
        }

        protected override WorkUnitPriorities GetPriority()
        {
            return TaskHelpers.GetTaskPriority(We.JiraIssue.Fields.Priority.Id);
        }

        protected override int GetReporterProfileId()
        {
            return 0;
        }

        protected override WorkUnitTypes GetTaskType()
        {
            return TaskHelpers.GetTaskType(We.JiraIssue.Fields.IssueType.Id);
        }

        protected IntegrationField GetRewardStatus()
        {
            return GetIntegrationFields().FirstOrDefault(x => x.Key == ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_ + GetExternalProjectId());
        }

        protected override bool IsCompleted()
        {
            if (GetRewardStatus() == null)
            {
                throw new ApplicationException($"Jira project with Id: {GetExternalProjectId()} is not connected to any tayra segments");
            }
            //return We.JiraIssue.Fields.Status.Id == GetRewardStatus().Value;
            return Data.RewardStatusEnteredDateId != null;
        }

        protected override (int? rewardStatusEnteredDateId, int? autoTimeSpentInMinutes) ParseChangelogData()
        {
            if (Mode == TaskConverterMode.TEST)
                return (DateHelper2.ToDateId(We.JiraIssue.Fields.StatusCategoryChangeDate), (int?)null);

            var jiraConnector = new AtlassianJiraConnector(null, DbContext, null, Config);
            Guid? integrationId = IntegrationHelpers.GetIntegrationId(DbContext, GetExternalProjectId(), GetIntegrationType());
            if (!integrationId.HasValue)
            {
                throw new ApplicationException($"Jira project with Id: {GetExternalProjectId()} is not connected to any tayra segments");
            }
            var statuses = jiraConnector.GetIssueStatuses(integrationId.Value, GetExternalProjectId(), We.JiraIssue.Fields.IssueType.Id);
            var todoStatuses = statuses.Where(x => x.Category.Id == IssueStatusCategories.ToDo).ToList();
            var inProgressStatuses = statuses.Where(x => x.Category.Id == IssueStatusCategories.InProgress).ToList();
            var doneStatuses = statuses.Where(x => x.Category.Id == IssueStatusCategories.Done).ToList();
            var rewardStatus = GetRewardStatus();

            //GetIssueWithChangelogs is limited to 100 changelogs, if there are more, we need to make additional API call for that task
            var changelogs = We.JiraIssue.TaskChangelogs != null && We.JiraIssue.TaskChangelogs.Count() < 100
                ? We.JiraIssue.TaskChangelogs
                : jiraConnector.GetIssueChangelog(integrationId.Value, GetExternalId(), "status");

            DateTime? enteredInProgress = null;
            DateTime? enteredRewardStatus = null;
            DateTime? fakeEnteredRewardStatus = null;
            foreach (var cl in changelogs)
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
                    fakeEnteredRewardStatus = cl.Created;
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

            if (!enteredRewardStatus.HasValue)
            {
                if (!fakeEnteredRewardStatus.HasValue)
                {
                    return (null, null);
                }

                return (
                    rewardStatusEnteredDateId: fakeEnteredRewardStatus.HasValue
                        ? DateHelper2.ToDateId(fakeEnteredRewardStatus.Value)
                        : (int?)null,
                    autoTimeSpentInMinutes: null);
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

            return (rewardStatusEnteredDateId: fakeEnteredRewardStatus.HasValue ? DateHelper2.ToDateId(fakeEnteredRewardStatus.Value) : (int?)null, autoTimeSpentInMinutes: (int)TimeSpan.FromHours((days * 8) + Math.Min(8, hours)).TotalMinutes);
        }

        protected override string GetIssueStatusName()
        {
            return We.JiraIssue.Fields.Status.Name;
        }

        protected override string GetIssueUrl()
        {
            var jiraSiteName = GetIntegrationFields().FirstOrDefault(x => x.Key == ATConstants.AT_SITE_NAME)?.Value;
            return $"https://{jiraSiteName}.atlassian.net/browse/{GetExternalId()}";
        }

        protected override int? GetTimeSpentInMinutes()
        {
            return We.JiraIssue.Fields.Timespent / 60;
        }
    }
}