using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Tayra.Common;
using Tayra.Connectors.Atlassian;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices
{
    public class SyncIssuesLoader : BaseLoader
    {
        #region Private Variables

        private readonly IShardMapProvider _shardMapProvider;

        #endregion

        #region Constructor

        public SyncIssuesLoader(IShardMapProvider shardMapProvider, LogService logService, CatalogDbContext catalogDb) : base(logService, catalogDb)
        {
            _shardMapProvider = shardMapProvider;
        }

        #endregion

        #region Public Methods

        public override void Execute(DateTime date, JObject requestBody, params Tenant[] tenants)
        {
            foreach (var tenant in tenants)
            {
                LogService.SetOrganizationId(tenant.Key);
                using (var organizationDb = new OrganizationDbContext(null, new ShardTenantProvider(tenant.Key), _shardMapProvider))
                {
                    PullIssues(organizationDb, date, LogService, requestBody);
                }
            }
        }

        public static void PullIssues(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService, JObject requestBody)
        {
            var syncReq = requestBody.ToObject<SyncRequest>();

            if (syncReq?.Params == null
            || !syncReq.Params.TryGetValue("jiraProjectId", out string jiraProjectId))
            {
                throw new ApplicationException("param jiraProjectId not provided");
            }
            
            var rewardStatusField = organizationDb
                                .IntegrationFields
                                .LastOrDefault(x => x.Key == ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_ + jiraProjectId);
            
            if (rewardStatusField == null)
            {
                throw new ApplicationException($"Jira project with Id: {jiraProjectId} is not connected to any tayra segments");
            }

            var jiraConnector = new AtlassianJiraConnector(null, organizationDb);

            var tasks = jiraConnector.GetBulkIssuesWithChangelog(rewardStatusField.IntegrationId, jiraProjectId);
            var profilesService = new ProfilesService(null, null, null, organizationDb);
            var tasksService = new TasksService(organizationDb);

            foreach (var task in tasks)
            {
                var fields = task.Fields;

                var assigneProfile = fields?.Assignee == null ? null : profilesService.GetMemberByExternalId(fields.Assignee.AccountId, IntegrationType.ATJ);
                var profileAssignment = assigneProfile == null ? null : organizationDb.ProfileAssignments.FirstOrDefault(x => x.ProfileId == assigneProfile.Id); //TODO: we need to append segmentId to webhooks
                var currentSegmentId = profileAssignment != null ? profileAssignment.SegmentId : (int?)null;

                var jiraBaseUrl = task.Self.Substring(0, task.Self.IndexOf('/', 10)); //TODO: is 10 ok for all integration types?
                if (assigneProfile == null || fields.Status.Id != rewardStatusField.Value)
                {
                        tasksService.AddOrUpdate(new TaskAddOrUpdateDTO
                        {
                            ExternalId = task.Key,
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

                        organizationDb.SaveChanges();
                        continue;
                }

                //if we came here assigneeProfile is not null
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
                    foreach (var cl in task.Changelog.Histories.Where(x => x.Items.Any(y => y.Field == "status")))
                    {
                        foreach (var i in cl.Items)
                        {
                            //if from todo to inProgress
                            if (todoStatuses.Select(x => x.Id).Contains(i.From) &&
                            inProgressStatuses.Select(x => x.Id).Contains(i.To))
                            {
                                enteredInProgress = cl.Created;
                            }
                            else if (i.To == rewardStatus.Value)
                            {
                                enteredRewardStatus = cl.Created;
                            }
                            //back in progress from rewardId
                            else if (i.From == rewardStatus.Value &&
                            !doneStatuses.Select(x => x.Id).Contains(i.To))
                            {
                                enteredInProgress = cl.Created;
                                enteredRewardStatus = null;
                            }
                            //back in progress from done
                            else if (doneStatuses.Select(x => x.Id).Contains(i.From) &&
                            i.To != rewardStatus.Value.ToString())
                            {
                                enteredInProgress = cl.Created;
                                enteredRewardStatus = null;
                            }
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

                tasksService.AddOrUpdate(new TaskAddOrUpdateDTO
                {
                    ExternalId = task.Key,
                    ExternalProjectId = fields.Project.Id,
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
                    AssigneeExternalId = task.Fields.Assignee.AccountId,
                    AssigneeProfileId = assigneProfile.Id,
                    ReporterProfileId = 0,
                    TeamId = profileAssignment?.TeamId,
                    SegmentId = currentSegmentId
                });

                organizationDb.SaveChanges();
            }
        }

        #endregion
    }
}
