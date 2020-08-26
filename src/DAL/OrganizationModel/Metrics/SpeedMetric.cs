using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class SpeedMetric : Metric
    {
        public SpeedMetric(int dateId) : base(MetricTypes.Speed, dateId)
        {
            Value = 5f;
        }
    }
}