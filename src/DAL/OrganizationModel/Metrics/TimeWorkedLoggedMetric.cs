using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class TimeWorkedLoggedMetric : SegmentMetric
    {
        public TimeWorkedLoggedMetric(IEnumerable<Task> tasks, int dateId, int segmentId): base(MetricType.TimeWorkedLogged, dateId, segmentId)
        {
            Value = tasks.Sum(x => x.TimeSpentInMinutes) ?? 0f;
        }
    }
}