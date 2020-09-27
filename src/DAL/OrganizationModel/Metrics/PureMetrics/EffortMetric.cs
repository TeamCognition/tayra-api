using System.Collections.Generic;
using System.Linq;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class EffortMetric : PureMetric
    {
        public EffortMetric(string name, int value) : base(name, value)
        {
            
        }
        public MetricShardWEntity Create(IEnumerable<Task> tasks, int dateId, int segmentId) => new MetricShardWEntity(tasks.Sum(x => x.EffortScore) ?? 0f, dateId, segmentId, this);

        public MetricShardWEntity[] CreateForEverySegment(IEnumerable<Task> tasks, int dateId)
        {
            return tasks
                .Where(x => x.SegmentId.HasValue)
                .GroupBy(x => x.SegmentId)
                .Select(s => this.Create(s.AsEnumerable(), dateId, s.Key.Value))
                .ToArray();
        }
    }
}