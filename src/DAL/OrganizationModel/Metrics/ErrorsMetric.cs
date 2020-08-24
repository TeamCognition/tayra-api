using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ErrorsMetric : Metric
    {
        public ErrorsMetric(float bugSeverity, float bugPopulationAffect): base(MetricTypes.Errors, bugSeverity * bugPopulationAffect)
        {
        }
    }
}