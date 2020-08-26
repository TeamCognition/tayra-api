using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class AssistMetric : Metric
    {
        public AssistMetric(PraisesReceivedMetric praisesReceivedMetric, int dateId): base(MetricTypes.Assist, dateId)
        {
            Value = praisesReceivedMetric.Value;
        }
    }
}