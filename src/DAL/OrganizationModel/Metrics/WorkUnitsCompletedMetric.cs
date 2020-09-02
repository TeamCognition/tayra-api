using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class WorkUnitsCompletedMetric : SegmentMetric
    {
        public WorkUnitsCompletedMetric(IEnumerable<Task> tasks, int dateId, int segmentId): base(MetricType.TasksCompleted, dateId, segmentId)
        {
            Value = tasks.Count(x => x.Status == TaskStatuses.Done);
        }
    }
}