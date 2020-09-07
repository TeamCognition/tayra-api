using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class WorkUnitsCompletedMetric : MetricWithSegment
    {
        public WorkUnitsCompletedMetric(IEnumerable<Task> tasks, int dateId, int segmentId) : base(MetricType.TasksCompleted, dateId, segmentId)
        {
            Value = tasks.Count(x => x.Status == TaskStatuses.Done);
        }

        public static WorkUnitsCompletedMetric[] CreateForEverySegment(IEnumerable<Task> tasks, int dateId)
        {
            return tasks
                .Where(x => x.SegmentId.HasValue)
                .GroupBy(x => x.SegmentId)
                .Select(s => new WorkUnitsCompletedMetric(s.AsEnumerable(), dateId, s.Key.Value))
                .ToArray();
        }
    }
}