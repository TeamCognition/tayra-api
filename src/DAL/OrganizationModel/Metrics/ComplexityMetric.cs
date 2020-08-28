using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ComplexityMetric : Metric
    {
        public ComplexityMetric(IEnumerable<Task> tasks, int dateId): base(MetricTypes.Complexity, dateId)
        {
            Value = tasks.Sum(x => x.Complexity);
        }
    }
}