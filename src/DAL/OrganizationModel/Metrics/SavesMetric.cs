using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class SavesMetric : SegmentMetric
    {
        public SavesMetric(IEnumerable<Task> tasks, int dateId, int segmentId): base(MetricType.Saves, dateId, segmentId)
        {
            Value = tasks.Count(x => x.IsProductionBugFixing && x.BugSeverity > 3);
        }
    }
}