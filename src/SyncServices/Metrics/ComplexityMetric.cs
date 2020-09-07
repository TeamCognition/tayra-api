using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class ComplexityMetric : MetricWithSegment
    {
        private ComplexityMetric(IEnumerable<Task> tasks, int dateId, int segmentId) : base(MetricType.Complexity, dateId, segmentId)
        {
            Value = tasks.Sum(x => x.Complexity);
        }

        public static ComplexityMetric[] CreateForEverySegment(IEnumerable<Task> tasks, int dateId)
        {
            return tasks
                .Where(x => x.SegmentId.HasValue)
                .GroupBy(x => x.SegmentId)
                .Select(s => new ComplexityMetric(s.AsEnumerable(), dateId, s.Key.Value))
                .ToArray();
        }
    }
}