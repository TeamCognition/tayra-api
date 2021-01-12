using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class Task : EntityGuidId, ITimeStampedEntity
    {
        public string ExternalId { get; set; }
        /// <summary>
        /// ex. jira project id
        /// </summary>
        public string ExternalProjectId { get; set; }

        public string ExternalUrl { get; set; }

        public IntegrationType IntegrationType { get; set; }

        public string Summary { get; set; }

        public TaskStatuses Status { get; set; }
        public TaskTypes Type { get; set; }

        public int? AutoTimeSpentInMinutes { get; set; }
        public int? TimeSpentInMinutes { get; set; }
        public int? TimeOriginalEstimatInMinutes { get; set; }

        public int? StoryPoints { get; set; }
        public int Complexity { get; set; }
        public TaskPriorities Priority { get; set; }

        public int? BugSeverity { get; set; }
        public float? BugPopulationAffect { get; set; } //population percentage affected by this bug 0-1

        public bool IsProductionBugCausing { get; set; } //is this task causing a production bug
        public bool IsProductionBugFixing { get; set; } //is this task fixing a production bug

        public float? EffortScore { get; set; }

        public string Labels { get; set; }

        public int LastModifiedDateId { get; set; }

        public int ReporterProfileId { get; set; }

        public string AssigneeExternalId { get; set; }

        public Guid? AssigneeProfileId { get; set; }
        public virtual Profile AssigneeProfile { get; set; }

        public Guid? TeamId { get; set; }
        public virtual Team Team { get; set; }

        public Guid? SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        public int? SegmentAreaId { get; set; }
        public virtual SegmentArea SegmentArea { get; set; }

        public int? TaskCategoryId { get; set; }
        public virtual TaskCategory TaskCategory { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
