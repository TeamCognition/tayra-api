using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class PowerMetric : Metric
    {
        public PowerMetric(int dateId) : base(MetricTypes.Power, dateId)
        {
            Value = 1f;
        }
    }
}