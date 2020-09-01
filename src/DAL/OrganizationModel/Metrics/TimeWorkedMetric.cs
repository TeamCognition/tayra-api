using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class TimeWorkedMetric : Metric
    {
        public TimeWorkedMetric(IEnumerable<Task> tasks, int dateId): base(MetricTypes.TimeWorked, dateId)
        {
            Value = tasks.Sum(x => x.AutoTimeSpentInMinutes) ?? 0f;
        }
    }
}