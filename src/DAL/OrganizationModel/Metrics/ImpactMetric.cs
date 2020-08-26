using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ImpactMetric : Metric
    {
        public ImpactMetric(int dateId) : base(MetricTypes.Impact, dateId)
        {
            Value = 1.5f;
        }
    }
}