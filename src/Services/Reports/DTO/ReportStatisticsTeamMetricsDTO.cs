using System;
using Tayra.Analytics;

namespace Tayra.Services
{
    public class ReportStatisticsTeamMetricsDTO
    {
        public MemberDTO[] Members { get; set; }

        public class MemberDTO
        {
            public Guid ProfileId { get; set; }
            public string Avatar { get; set; }
            public string Name { get; set; }
            public float[] HeatTrend { get; set; }
            public MetricDTO[] Metrics { get; set; }

            public class MetricDTO
            {
                public MetricTypesDelete Id { get; set; }
                public double Average { get; set; }
            }
        }
    }
}
