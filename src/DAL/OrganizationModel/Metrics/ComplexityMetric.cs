using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ComplexityMetric : Metric
    {
        public ComplexityMetric(IEnumerable<Task> tasks) : base(MetricTypes.Complexity)
        {
            Value = tasks.Sum(x => x.Complexity);
        }
    }
}