using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class ProfileSummaryGridDTO
    {
        public Guid ProfileId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public PlatformData PlatformInfo { get; set; }
        public Segment[] Segments { get; set; }
        public Team[] Teams { get; set; }
        public Integration[] Integrations { get; set; }

        public class PlatformData
        {
            public float CompanyTokens { get; set; }
            public int? Praises { get; set; }
            public int? CompletedQuests { get; set; } = 0;
        }

        public class Team
        {
            public string Name { get; set; }
            public string Key { get; set; }
            public DateTime JoinDate { get; set; }
        }

        public class Segment
        {
            public string Name { get; set; }
            public string Key { get; set; }
            public DateTime JoinDate { get; set; }

        }

        public class Integration
        {
            public IntegrationType Type { get; set; }
            public DateTime IntegratedOn { get; set; }

        }
    }
}