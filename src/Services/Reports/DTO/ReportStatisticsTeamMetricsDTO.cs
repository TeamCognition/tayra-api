using Tayra.Common;

namespace Tayra.Services
{
    public class ReportStatisticsTeamMetricsDTO
    {
        public MemberDTO[] Members { get; set; }

        public class MemberDTO
        {
            public int ProfileId { get; set; }
            public string Avatar { get; set; }
            public string Name { get; set; }
            public MetricDTO[] Metrics { get; set; }

            public class MetricDTO
            {
                public MetricTypes Id { get; set; }
                public double Average { get; set; }
            }
        }
    }
}
