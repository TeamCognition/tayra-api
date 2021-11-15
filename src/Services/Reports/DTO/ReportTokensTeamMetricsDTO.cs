using System;

namespace Tayra.Services
{
    public class ReportTokensTeamMetricsDTO
    {
        public TeamDTO[] Teams { get; set; }
        public DataDTO[] Data { get; set; }

        public class DataDTO
        {
            public int DateId { get; set; }
            public float Value { get; set; }
        }

        public class TeamDTO
        {
            public Guid TeamId { get; set; }
            public double TokensEarned { get; set; }
            public double TokensSpent { get; set; }
        }
    }
}
