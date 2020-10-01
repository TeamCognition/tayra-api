using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class ComplexityMetric : PureMetric
    {
        public ComplexityMetric(string name, int value) : base(name, value)
        {
            
        }
        public MetricShardWEntity Create(IEnumerable<Task> tasks, int dateId, int segmentId) => new MetricShardWEntity(tasks.Sum(x => x.Complexity), dateId, segmentId, this);
        
        public MetricShardWEntity[] CreateForEverySegment(IEnumerable<Task> tasks, int dateId)
        {
            return tasks
                .Where(x => x.SegmentId.HasValue)
                .GroupBy(x => x.SegmentId)
                .Select(s => this.Create(s.AsEnumerable(), dateId, s.Key.Value))
                .ToArray();
        }
        
        public override object[] GetRawMetrics(OrganizationDbContext db, DatePeriod period, int entityId, EntityTypes entityType)
        {
            var profileIds = GetProfileIds(db, entityId, entityType);
            return (from t in db.Tasks
                where t.Status == TaskStatuses.Done
                where t.SegmentId.HasValue
                where profileIds.Contains(t.AssigneeProfileId.Value)
                where t.LastModifiedDateId >= period.FromId && t.LastModifiedDateId <= period.ToId
                select new RawMetric
                {
                    Assignee = new TableData.Profile($"{t.AssigneeProfile.FirstName} {t.AssigneeProfile.LastName}",
                        t.AssigneeProfile.Username),
                    Key = t.ExternalId,
                    Summary = new TableData.ExternalLink(t.Summary, t.ExternalUrl),
                    Complexity = t.Complexity,
                    Priority = t.Priority,
                    TimeLogged = new TableData.TimeInMinutes(t.TimeSpentInMinutes),
                    LastModifiedAt = t.LastModified ?? t.Created,
                    CreatedAt = t.Created
                }).ToArray<object>();
        }

        public override Type TypeOfRawMetric => typeof(RawMetric);
        
        public class RawMetric
        {
            public TableData.Profile Assignee { get; set; }
            public string Key { get; set; }
            public TableData.ExternalLink Summary { get; set; }
            public int Complexity { get; set; }
            public TaskPriorities Priority { get; set; }
            public TableData.TimeInMinutes TimeLogged { get; set; }
            public DateTime LastModifiedAt { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}

