using Tayra.Common;

namespace Tayra.SyncServices.Metrics
{
    public class EffortMetric : Metric
    {
        public EffortMetric(float value): base(MetricTypes.Assist)
        {
            Value = value;
        }
    }
}