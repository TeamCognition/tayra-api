using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class EffortMetric : Metric
    {
        public EffortMetric(IEnumerable<Task> tasks, int dateId): base(MetricTypes.Effort, dateId)
        {
            Value = tasks.Sum(x => x.EffortScore) ?? 0f;
        }
    }
}