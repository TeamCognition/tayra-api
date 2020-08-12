using Tayra.Common;

namespace Tayra.Services
{
    public class SegmentRankChartDTO
    {
        public RankChartMetricDTO[] Metrics { get; set; }
        
        public class RankChartMetricDTO
        {
            public MetricTypes Id { get; set; }
            public MemberRankDTO[] MemberValues { get; set; }

            public class MemberRankDTO
            {
                public int ProfileId { get; set; }
                public float Value { get; set; }
            }
        }
    }
}