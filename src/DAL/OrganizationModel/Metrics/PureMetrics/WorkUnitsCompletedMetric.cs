using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Metrics
{
    public class WorkUnitsCompletedMetric : PureMetric
    {
        public WorkUnitsCompletedMetric(string name, int value) : base(name, value)
        {

        }
        public MetricShardWEntity Create(IEnumerable<WorkUnit> tasks, int dateId, Guid segmentId) => new MetricShardWEntity(tasks.Count(x => x.Status == WorkUnitStatuses.Done), dateId, segmentId, this);

        public MetricShardWEntity[] CreateForEverySegment(IEnumerable<WorkUnit> tasks, int dateId)
        {
            return tasks
                .Where(x => x.SegmentId.HasValue)
                .GroupBy(x => x.SegmentId)
                .Select(s => this.Create(s.AsEnumerable(), dateId, s.Key.Value))
                .ToArray();
        }

        public override object[] GetRawMetrics(OrganizationDbContext db, DatePeriod period, Guid entityId, EntityTypes entityType)
        {
            var profileIds = GetProfileIds(db, entityId, entityType);
            return (from t in db.Tasks
                    where t.Status == WorkUnitStatuses.Done
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
                        LastModifiedAt = new TableData.DateInSeconds(t.LastModified ?? t.Created),
                        CreatedAt = new TableData.DateInSeconds(t.Created)
                    }).ToArray<object>();
        }

        public override Type TypeOfRawMetric => typeof(RawMetric);

        public class RawMetric
        {
            public TableData.Profile Assignee { get; set; }
            public string Key { get; set; }
            public TableData.ExternalLink Summary { get; set; }
            public int Complexity { get; set; }
            public WorkUnitPriorities Priority { get; set; }
            public TableData.TimeInMinutes TimeLogged { get; set; }
            public TableData.DateInSeconds LastModifiedAt { get; set; }
            public TableData.DateInSeconds CreatedAt { get; set; }
        }
    }
}