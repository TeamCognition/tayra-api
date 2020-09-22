using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class ComplexityMetric : PureMetricWithSegment
    {
        private ComplexityMetric(float value, int dateId, int segmentId) : base(MetricType.Complexity, value, dateId, segmentId )
        {
            
        }
        public static ComplexityMetric Create(IEnumerable<Task> tasks, int dateId, int segmentId) => new ComplexityMetric(tasks.Sum(x => x.Complexity), dateId, segmentId);
        
        public static ComplexityMetric[] CreateForEverySegment(IEnumerable<Task> tasks, int dateId)
        {
            return tasks
                .Where(x => x.SegmentId.HasValue)
                .GroupBy(x => x.SegmentId)
                .Select(s => ComplexityMetric.Create(s.AsEnumerable(), dateId, s.Key.Value))
                .ToArray();
        }
    }
}