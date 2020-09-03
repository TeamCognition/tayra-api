using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class AssistMetric : Metric
    {
        public AssistMetric(PraisesReceivedMetric praisesReceivedMetric) : base(MetricType.Assists, praisesReceivedMetric.DateId)
        {
            Value = praisesReceivedMetric.Value;
        }
    }
}