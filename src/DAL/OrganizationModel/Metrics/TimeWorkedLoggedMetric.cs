using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class TimeWorkedLoggedMetric : Metric
    {
        public TimeWorkedLoggedMetric(IEnumerable<Task> tasks, int dateId): base(MetricTypes.TimeWorkedLogged, dateId)
        {
            Value = tasks.Sum(x => x.TimeSpentInMinutes) ?? 0f;
        }
    }
}