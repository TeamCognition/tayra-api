using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class WorkUnitsCompletedMetric : Metric
    {
        public WorkUnitsCompletedMetric(IEnumerable<Task> tasks, int dateId): base(MetricTypes.WorkUnitsCompleted, dateId)
        {
            Value = tasks.Count(x => x.Status == TaskStatuses.Done);
        }
    }
}