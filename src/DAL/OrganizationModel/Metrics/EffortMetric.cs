using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class EffortMetric : SegmentMetric
    {
        public EffortMetric(IEnumerable<Task> tasks, int dateId, int segmentId): base(MetricType.Effort, dateId, segmentId)
        {
            Value = tasks.Sum(x => x.EffortScore) ?? 0f;
        }
    }
}