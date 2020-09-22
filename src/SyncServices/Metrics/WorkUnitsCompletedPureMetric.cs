using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class WorkUnitsCompletedPureMetric : PureMetricWithSegment
    {
        private WorkUnitsCompletedPureMetric(float value, int dateId, int segmentId) : base(MetricType.TasksCompleted, value, dateId, segmentId )
        {
            
        }
        public static WorkUnitsCompletedPureMetric Create(IEnumerable<Task> tasks, int dateId, int segmentId) => new WorkUnitsCompletedPureMetric(tasks.Count(x => x.Status == TaskStatuses.Done), dateId, segmentId);
        
        public static WorkUnitsCompletedPureMetric[] CreateForEverySegment(IEnumerable<Task> tasks, int dateId)
        {
            return tasks
                .Where(x => x.SegmentId.HasValue)
                .GroupBy(x => x.SegmentId)
                .Select(s => WorkUnitsCompletedPureMetric.Create(s.AsEnumerable(), dateId, s.Key.Value))
                .ToArray();
        }
    }
}