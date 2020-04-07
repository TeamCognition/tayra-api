
using System;
using System.Linq;
using Firdaws.Core;
using Tayra.Common;
using Tayra.Connectors.Atlassian;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Models.Organizations;

namespace Tayra.Services.TaskConverters
{
    public class TaskConverterJira : TaskConverterBase
    {
        protected WebhookEvent We;
        protected IntegrationField RewardStatusCache;
        protected bool UseConnector;
        public TaskConverterJira(OrganizationDbContext dbContext,
                                 IProfilesService profilesService,
                                 WebhookEvent we,
                                 bool useConnector = true)
                                 : base(dbContext, profilesService)
        {
            We = we;
            UseConnector = useConnector;
        }

        public override string GetExternalId()
        {
            return We.JiraIssue.Key;
        }

        public override IntegrationType GetIntegrationType()
        {
            return IntegrationType.ATJ;
        }

        public override IssueStatusCategories GetJiraStatusCategory()
        {
            return We.JiraIssue.Fields.Status.Category.Id;
        }

        public override string GetExternalProjectId()
        {
            return We.JiraIssue.Fields.Project.Id;
        }

        public override int? GetStoryPoints()
        {
            return (int?)We.JiraIssue.Fields.StoryPointsCF;
        }

        public override string GetSummary()
        {
            return We.JiraIssue.Fields.Summary;
        }

        public override int? GetTimeOriginalEstimateInMinutes()
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

        internal override int? GetAutoTimeSpentInMinutes()
        {
            if (UseConnector)
            {
                var jiraConnector = new AtlassianJiraConnector(null, DbContext);
                var issueChangelogs = jiraConnector.GetIssueChangelog(GetRewardStatus().IntegrationId, GetExternalId(), "status");

                var statuses = jiraConnector.GetIssueStatuses(GetRewardStatus().IntegrationId, GetExternalProjectId(), We.JiraIssue.Fields.IssueType.Id);
                var todoStatuses = statuses.Where(x => x.Category.Id == IssueStatusCategories.ToDo).ToList();
                var inProgressStatuses = statuses.Where(x => x.Category.Id == IssueStatusCategories.InProgress).ToList();
                var doneStatuses = statuses.Where(x => x.Category.Id == IssueStatusCategories.Done).ToList();
                var rewardStatus = GetRewardStatus();

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

        internal override string GetIssueStatusName()
        {
            return We.JiraIssue.Fields.Status.Name;
        }

        internal override string GetIssueUrl()
        {
            var jiraBaseUrl = We.JiraIssue.Self.Substring(0, We.JiraIssue.Self.IndexOf('/', 10)); //TODO: is 10 ok for all integration types?
            return jiraBaseUrl + "/browse/" + GetExternalId();
        }

        internal override int? GetTimeSpentInMinutes()
        {
            return We.JiraIssue.Fields.Timespent / 60;
        }
    }
}