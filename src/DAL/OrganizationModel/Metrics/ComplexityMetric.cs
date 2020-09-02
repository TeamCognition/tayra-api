using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ComplexityMetric : SegmentMetric
    {
        public ComplexityMetric(IEnumerable<Task> tasks, int dateId, int segmentId): base(MetricType.Complexity, dateId, segmentId)
        {
            Value = tasks.Sum(x => x.Complexity);
        }
    }
}