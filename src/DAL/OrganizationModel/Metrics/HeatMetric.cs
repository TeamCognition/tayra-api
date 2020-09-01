using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class HeatMetric : Metric
    {
        public HeatMetric(int dateId) : base(MetricTypes.Heat, dateId)
        {
            Value = 10f;
        }
    }
}