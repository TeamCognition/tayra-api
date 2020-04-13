using System;
using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Connectors.Atlassian;
using Tayra.Models.Organizations;

namespace Tayra.Services.TaskConverters
{
    public abstract class TaskConverterBase
    {
        protected OrganizationDbContext DbContext;
        protected ProfileAssignment CachedProfileAssignment;
        public TaskAddOrUpdateDTO Data {get; protected set;}
        protected IProfilesService ProfilesService;
        protected Profile AssigneeProfile;
        protected bool BasicTaskDataUpdated { get; private set; }
        protected double? EffortScoreDiff;
        public TaskConverterBase(OrganizationDbContext dbContext, IProfilesService profilesService)
        {
            DbContext = dbContext;
            ProfilesService = profilesService;
        }
        public void UpdateBasicTaskData()
        {
            BasicTaskDataUpdated = true;
            if (GetAssigneeExternalId() != null)
            {
                AssigneeProfile = ProfilesService.GetMemberByExternalId(GetAssigneeExternalId(), GetIntegrationType());
            }
            Data = new TaskAddOrUpdateDTO
            {
                ExternalId = GetExternalId(),
                ExternalProjectId = GetExternalProjectId(),
                IntegrationType = GetIntegrationType(),
                Summary = GetSummary(),
                JiraStatusCategory = GetJiraStatusCategory(),
                TimeSpentInMinutes = GetTimeSpentInMinutes(),
                TimeOriginalEstimatInMinutes = GetTimeOriginalEstimateInMinutes(),
                StoryPoints = GetStoryPoints(),
                Priority = GetPriority(),
                Type = GetTaskType(),
                Labels = GetLabels(),
                AssigneeExternalId = GetAssigneeExternalId(),
                AssigneeProfileId = GetAssigneeProfileId(),
                ReporterProfileId = GetReporterProfileId(),
                TeamId = GetTeamId(),
                SegmentId = GetCurrentSegmentId(),
                LastModifiedDateId = GetLastModifiedDateId()
            };
        }

        public virtual bool ShouldBeProcessed()
        {
            return true;
        }
        protected abstract int? GetTimeSpentInMinutes();

        public void EnsureBasicDataIsFilled()
        {
            if (!BasicTaskDataUpdated)
            {
                UpdateBasicTaskData();
            }
        }
        public bool IsEligableForExtraData()
        {
            return Data.AssigneeProfileId.HasValue && IsCompleted();
        }
        public void FillExtraDataIfCompleted()
        {
            EnsureBasicDataIsFilled();
            if (IsEligableForExtraData())
            {
                Data.AutoTimeSpentInMinutes = GetAutoTimeSpentInMinutes();
                FillEffortScore();
            }
        }

        public void ConcludeActionPointsIfPossible(IAdvisorService advisorService)
        {
            if (advisorService != null)
            {
                EnsureBasicDataIsFilled();
                if (IsEligableForExtraData())
                {
                    var aps = DbContext.ActionPoints.Where(x =>
                            x.ProfileId == Data.AssigneeProfileId.Value &&
                            x.ConcludedOn == null &&
                            (x.Type == ActionPointTypes.ProfilesNoCompletedTasksIn1Week ||
                            x.Type == ActionPointTypes.ProfilesNoCompletedTasksIn2Week)
                        )
                        .Select(ap => ap.Id)
                        .ToArray();
                    advisorService.ConcludeActionPoints(GetCurrentSegmentId().Value, aps, null);
                }
            }
        }

        public void AddNecessaryTokensIfPossible(ITokensService tokensService)
        {
            EnsureBasicDataIsFilled();
            if (IsEligableForExtraData())
            {
                if (EffortScoreDiff > 0)
                {
                    DoAddTokens(EffortScoreDiff.Value, tokensService);
                }
            }
        }

        // Intended to be overridable
        protected virtual void DoAddTokens(double effortScoreDiff, ITokensService tokensService)
        {
            tokensService.CreateTransaction(TokenType.CompanyToken, Data.AssigneeProfileId.Value, effortScoreDiff, TransactionReason.JiraIssueCompleted, ClaimBundleTypes.EarnedFromWork);
            tokensService.CreateTransaction(TokenType.Experience, Data.AssigneeProfileId.Value, effortScoreDiff, TransactionReason.JiraIssueCompleted, ClaimBundleTypes.EarnedFromWork);
        }

        protected virtual void FillEffortScore()
        {
            if (Data.AutoTimeSpentInMinutes.HasValue || Data.TimeSpentInMinutes.HasValue)
            {
                Data.EffortScore = TayraEffortCalculator.CalcEffortScore(
                    Data.TimeSpentInMinutes,
                    Data.AutoTimeSpentInMinutes,
                    TayraPersonalPerformance.MapSPToComplexity(Data.StoryPoints ?? 0)
                );
                var task = DbContext.Tasks.FirstOrDefault(x => x.ExternalId == Data.ExternalId && x.IntegrationType == Data.IntegrationType);
                EffortScoreDiff = Data.TimeSpentInMinutes == null || task?.EffortScore == null ?
                    Data.EffortScore.Value :
                    Math.Max(0, Data.EffortScore.Value - task.EffortScore.Value);
            }
        }

        protected abstract int? GetAutoTimeSpentInMinutes();
        protected abstract bool IsCompleted();
        protected abstract int? GetLastModifiedDateId();

        protected ProfileAssignment GetProfileAssignment()
        {
            if (CachedProfileAssignment == null)
            {
                if (GetAssigneeProfileId() != null)
                {
                    CachedProfileAssignment = DbContext.ProfileAssignments.FirstOrDefault(x => x.ProfileId == GetAssigneeProfileId().Value);
                }
            }
            return CachedProfileAssignment;
        }

        protected int? GetCurrentSegmentId()
        {
            return GetProfileAssignment()?.SegmentId;
        }

        protected int? GetTeamId()
        {
            return GetProfileAssignment()?.TeamId;
        }

        protected abstract int GetReporterProfileId();
        protected int? GetAssigneeProfileId()
        {
            return AssigneeProfile?.Id;
        }
        protected abstract string GetAssigneeExternalId();
        protected abstract string[] GetLabels();
        protected abstract TaskTypes GetTaskType();
        protected abstract TaskPriorities GetPriority();
        protected abstract string GetExternalId();
        protected abstract string GetExternalProjectId();
        protected abstract IntegrationType GetIntegrationType();
        protected abstract string GetSummary();
        virtual protected IssueStatusCategories GetJiraStatusCategory()
        {
            return IssueStatusCategories.NoCategory;
        }
        protected abstract int? GetTimeOriginalEstimateInMinutes();
        protected abstract int? GetStoryPoints();

        public virtual void LogIfPossible(ILogsService logsService)
        {
            if (AssigneeProfile != null && logsService != null)
            {
                LogEvents eventType = IsCompleted() ? LogEvents.StatusChangeToCompleted : LogEvents.IssueStatusChange;
                var logData = new LogCreateDTO
                {
                    Event = eventType,
                    Data = new Dictionary<string, string>
                    {
                        { "timestamp", DateTime.UtcNow.ToString() },
                        { "issueUrl", GetIssueUrl() },
                        { "issueKey", GetExternalId() },
                        { "issueSummary", GetSummary() },
                        { "issueStatus", GetIssueStatusName() },
                        { "profileUsername", AssigneeProfile.Username },
                        { "timespent", TayraEffortCalculator.GetEffectiveTimeSpent(Data.TimeSpentInMinutes, Data.AutoTimeSpentInMinutes).ToString() }
                    },
                    ProfileId = AssigneeProfile.Id
                };
                if (EffortScoreDiff.HasValue)
                {
                    logData.Data.Add("effortScore", Math.Round(EffortScoreDiff.Value, 2).ToString());
                }
                logsService.LogEvent(logData);
            }
        }

        protected abstract string GetIssueStatusName();
        protected abstract string GetIssueUrl();
    }
}