using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class PraisesReceivedMetric : Metric
    {
        public PraisesReceivedMetric(float value) : base(type: MetricTypes.PraisesReceived, value)
        {

        }
    }
}