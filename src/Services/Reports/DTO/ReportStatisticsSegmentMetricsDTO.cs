using Tayra.Common;

namespace Tayra.Services
{
    public class ReportStatisticsSegmentMetricsDTO
    {
        public MetricDTO[] Metrics { get; set; }

        public class MetricDTO
        {
            public MetricTypes MetricId { get; set; }
            public DataDTO[] Data { get; set; }

            public class DataDTO
            {
                public int DateId { get; set; }
                public float Average { get; set; }
            }
        }
    }
}