using Tayra.Common;

namespace Tayra.SyncServices.Metrics
{
    public class ErrorsMetric : Metric
    {
        public ErrorsMetric(float bugSeverity, float bugPopulationAffect): base(MetricTypes.Errors)
        {
            Value = bugSeverity * bugPopulationAffect;
        }
    }
}