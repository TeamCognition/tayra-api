using Tayra.Common;

namespace Tayra.SyncServices.Metrics
{
    public class WorkUnitsCompleted : Metric
    {
        public WorkUnitsCompleted(float value) : base(MetricTypes.WorkUnitsCompleted)
        {
            Value = value;
        }
    }
}