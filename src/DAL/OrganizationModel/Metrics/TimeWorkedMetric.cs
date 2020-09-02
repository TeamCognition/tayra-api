using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class TimeWorkedMetric : SegmentMetric
    {
        public TimeWorkedMetric(IEnumerable<Task> tasks, int dateId, int segmentId): base(MetricType.TimeWorked, dateId, segmentId)
        {
            Value = tasks.Sum(x => x.AutoTimeSpentInMinutes) ?? 0f;
        }
    }
}