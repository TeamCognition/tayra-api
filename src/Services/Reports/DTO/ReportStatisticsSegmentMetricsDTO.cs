using Tayra.Analytics;

namespace Tayra.Services
{
    public class ReportStatisticsSegmentMetricsDTO
    {
        public MetricDTO[] Metrics { get; set; }

        public class MetricDTO
        {
            public MetricTypesDelete MetricId { get; set; }
            public DataDTO[] Data { get; set; }

            public class DataDTO
            {
                public int DateId { get; set; }
                public float Average { get; set; }
            }
        }
    }
}