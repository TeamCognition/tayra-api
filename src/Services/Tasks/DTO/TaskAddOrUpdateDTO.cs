using System;
using Tayra.Common;
using Tayra.Connectors.Atlassian;

namespace Tayra.Services
{
    public class TaskAddOrUpdateDTO
    {
        public string ExternalId { get; set; }
        /// <summary>
        /// jira project id
        /// </summary>
        public string ExternalProjectId { get; set; }

        public string ExternalUrl { get; set; }

        public IntegrationType IntegrationType { get; set; }

        public string Summary { get; set; }

        public string JiraStatusId { get; set; }
        public WorkUnitTypes Type { get; set; }

        public int? AutoTimeSpentInMinutes { get; set; }
        public int? TimeSpentInMinutes { get; set; }
        public int? TimeOriginalEstimateInMinutes { get; set; }

        public int? StoryPoints { get; set; }
        public WorkUnitPriorities Priority { get; set; }

        public double? EffortScore { get; set; }

        public string[] Labels { get; set; }

        public string AssigneeExternalId { get; set; }
        public Guid? AssigneeProfileId { get; set; }

        public int ReporterProfileId { get; set; }

        public Guid? TeamId { get; set; }

        public Guid? SegmentId { get; set; }

        public int? RewardStatusEnteredDateId { get; set; }
    }
}