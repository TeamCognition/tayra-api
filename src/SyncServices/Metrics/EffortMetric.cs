using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class EffortMetric : PureMetricWithSegment
    {
        private EffortMetric(float value, int dateId, int segmentId) : base(MetricType.Effort, value, dateId, segmentId )
        {
            
        }
        public static EffortMetric Create(IEnumerable<Task> tasks, int dateId, int segmentId) => new EffortMetric(tasks.Sum(x => x.EffortScore) ?? 0f, dateId, segmentId);

        public static EffortMetric[] CreateForEverySegment(IEnumerable<Task> tasks, int dateId)
        {
            return tasks
                .Where(x => x.SegmentId.HasValue)
                .GroupBy(x => x.SegmentId)
                .Select(s => EffortMetric.Create(s.AsEnumerable(), dateId, s.Key.Value))
                .ToArray();
        }
    }
}