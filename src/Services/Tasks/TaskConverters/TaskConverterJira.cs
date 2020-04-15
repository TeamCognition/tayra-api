
using System;
using System.Linq;
using Firdaws.Core;
using Tayra.Common;
using Tayra.Connectors.Atlassian;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Models.Organizations;

namespace Tayra.Services.TaskConverters
{
    public enum TaskConverterJiraMode
    {
        TEST,
        NORMAL,
        BULK
    }
    public class TaskConverterJira : TaskConverterBase
    {
        protected WebhookEvent We;
        protected IntegrationField RewardStatusCache;
        protected TaskConverterJiraMode Mode;
        public TaskConverterJira(OrganizationDbContext dbContext,
                                 IProfilesService profilesService,
                                 WebhookEvent we,
                                 TaskConverterJiraMode mode = TaskConverterJiraMode.NORMAL)
                                 : base(dbContext, profilesService)
        {
            Init(we, mode);
        }

        public TaskConverterJira(OrganizationDbContext dbContext,
                                 IProfilesService profilesService,
                                 JiraIssue jiraIssue,
                                 TaskConverterJiraMode mode = TaskConverterJiraMode.NORMAL)
                                 : base(dbContext, profilesService)
        {
            Init(new WebhookEvent {JiraIssue = jiraIssue}, mode);
        }

        private void Init(WebhookEvent we, TaskConverterJiraMode mode)
        {
            We = we;
            Mode = mode;
        }

        public override bool ShouldBeProcessed()
        {
            if (Mode == TaskConverterJiraMode.NORMAL)
            {
                var jiraConnector = new AtlassianJiraConnector(null, DbContext);
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

        protected override IssueStatusCategories GetJiraStatusCategory()
        {
            return We.JiraIssue.Fields.Status.Category.Id;
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

        protected override int? GetLastModifiedDateId()
        {
            return DateHelper2.ToDateId(We.JiraIssue.Fields.StatusCategoryChangeDate);
        }

        protected override TaskPriorities GetPriority()
        {
            return TaskHelpers.GetTaskPriority(We.JiraIssue.Fields.Priority.Id);
        }

        protected override int GetReporterProfileId()
        {
            return 0;
        }

        protected override TaskTypes GetTaskType()
        {
            return TaskHelpers.GetTaskType(We.JiraIssue.Fields.IssueType.Id);
        }

        protected IntegrationField GetRewardStatus()
        {
            if (RewardStatusCache == null)
            {
                RewardStatusCache = DbContext
                    .IntegrationFields
                    .LastOrDefault(x => x.Key == ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_ + GetExternalProjectId());
            }
            return RewardStatusCache;
        }

        protected override bool IsCompleted()
        {
            if (GetRewardStatus() == null)
            {
                throw new ApplicationException($"Jira project with Id: {GetExternalProjectId()} is not connected to any tayra segments");
            }
            return We.JiraIssue.Fields.Status.Id != GetRewardStatus().Value;
        }

        protected override int? GetAutoTimeSpentInMinutes()
        {
            if (Mode != TaskConverterJiraMode.TEST)
            {
                var jiraConnector = new AtlassianJiraConnector(null, DbContext);
                int? integrationId = IntegrationHelpers.GetIntegrationId(DbContext, GetExternalProjectId(), GetIntegrationType());
                if (!integrationId.HasValue)
                {
                    throw new ApplicationException($"Jira project with Id: {GetExternalProjectId()} is not connected to any tayra segments");
                }
                var statuses = jiraConnector.GetIssueStatuses(integrationId.Value, GetExternalProjectId(), We.JiraIssue.Fields.IssueType.Id);
                var todoStatuses = statuses.Where(x => x.Category.Id == IssueStatusCategories.ToDo).ToList();
                var inProgressStatuses = statuses.Where(x => x.Category.Id == IssueStatusCategories.InProgress).ToList();
                var doneStatuses = statuses.Where(x => x.Category.Id == IssueStatusCategories.Done).ToList();
                var rewardStatus = GetRewardStatus();

                var issueChangelogs = We.JiraIssue.TaskChangelogs;
                if (issueChangelogs == null || issueChangelogs.Count >= 100)
                {
                    issueChangelogs = jiraConnector.GetIssueChangelog(integrationId.Value, GetExternalId(), "status");
                }

                DateTime? enteredInProgress = null;
                DateTime? enteredRewardStatus = null;
                foreach (var cl in issueChangelogs)
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

                return (int)TimeSpan.FromHours((days * 8) + Math.Min(8, hours)).TotalMinutes;
            }
            return null;
        }

        protected override string GetIssueStatusName()
        {
            return We.JiraIssue.Fields.Status.Name;
        }

        protected override string GetIssueUrl()
        {
            var jiraBaseUrl = We.JiraIssue.Self.Substring(0, We.JiraIssue.Self.IndexOf('/', 10)); //TODO: is 10 ok for all integration types?
            return jiraBaseUrl + "/browse/" + GetExternalId();
        }

        protected override int? GetTimeSpentInMinutes()
        {
            return We.JiraIssue.Fields.Timespent / 60;
        }
    }
}