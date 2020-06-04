using System;
using System.Linq;
using Cog.Core;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class TasksService : BaseService<OrganizationDbContext>, ITasksService
    {
        #region Constructor

        public TasksService(OrganizationDbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        public void AddOrUpdate(TaskAddOrUpdateDTO dto)
        {
            var task = DbContext.Tasks.FirstOrDefault(x => x.ExternalId == dto.ExternalId && x.IntegrationType == dto.IntegrationType);

            if(task == null)
            {
                task = new Task
                {
                    ExternalId = dto.ExternalId,
                    ExternalProjectId = dto.ExternalProjectId,
                    IntegrationType = dto.IntegrationType
                };

                DbContext.Add(task);
            }

            task.Summary = dto.Summary;
            task.LastModifiedDateId = dto.LastModifiedDateId ?? DateHelper2.ToDateId(DateTime.UtcNow);
            task.Status = TayraPersonalPerformance.MapJiraIssueCategoryToTaskStatus(dto.JiraStatusCategory);
            task.Type = dto.Type;
            task.AutoTimeSpentInMinutes = dto.AutoTimeSpentInMinutes;
            task.TimeSpentInMinutes = dto.TimeSpentInMinutes;
            task.TimeOriginalEstimatInMinutes = dto.TimeOriginalEstimatInMinutes;
            task.StoryPoints = dto.StoryPoints;
            task.Complexity = TayraPersonalPerformance.MapSPToComplexity(dto.StoryPoints);
            task.BugSeverity = dto.Type == TaskTypes.Task ? (int?)null : TayraPersonalPerformance.MapPriorityToSeverity(dto.Priority);
            task.IsProductionBugFixing = task.BugSeverity > 3; //jira workaround
            task.Priority = dto.Priority;
            task.Labels = string.Join(',', dto.Labels);
            task.AssigneeExternalId = dto.AssigneeExternalId;
            task.AssigneeProfileId = dto.AssigneeProfileId;
            task.TeamId = dto.TeamId;
            task.SegmentId = dto.SegmentId;

            if(dto.EffortScore.HasValue)
            {
                task.EffortScore = (float?)dto.EffortScore;
            }
        }

        #endregion
    }
}
