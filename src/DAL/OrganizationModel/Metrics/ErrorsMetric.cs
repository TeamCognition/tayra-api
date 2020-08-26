using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ErrorsMetric : Metric
    {
        public ErrorsMetric(IEnumerable<Task> tasks, int dateId): base(MetricTypes.Errors, dateId)
        {
            Value = tasks.Where(x => x.IsProductionBugCausing).Sum(x => x.BugSeverity * x.BugPopulationAffect) ?? 0f;
        }
    }
}