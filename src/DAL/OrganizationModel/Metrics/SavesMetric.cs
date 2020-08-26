using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class SavesMetric : Metric
    {
        public SavesMetric(IEnumerable<Task> tasks, int dateId): base(MetricTypes.Saves, dateId)
        {
            Value = tasks.Count(x => x.IsProductionBugFixing && x.BugSeverity > 3);
        }
    }
}