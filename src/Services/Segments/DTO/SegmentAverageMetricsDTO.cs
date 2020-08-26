using Tayra.Common;

namespace Tayra.Services
{
    public class SegmentAverageMetricsDTO
    {
        public int LatestUpdateDateId { get; set; }
        public SegmentMetricDTO[] Metrics { get; set; }
        public class SegmentMetricDTO
        {
            public MetricTypes Id { get; set; }
            public float[] Averages { get; set; }
        }
    }
}