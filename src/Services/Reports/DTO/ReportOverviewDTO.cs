using System.Collections.Generic;
using Tayra.Analytics;
using Tayra.Common;

namespace Tayra.Services
{
    public class ReportOverviewDTO
    {
        public StatisticsDTO Statistics { get; set; }
        public MetricDTO[] Metrics { get;set; }
        public NodeDTO[] Nodes { get; set; }

        public class StatisticsDTO
        {
            public int ActiveTeams { get; set; }
            public int ActiveMembers { get; set; }
            public int ActiveQuests { get; set; }
            public int ActiveIntegrations { get; set; }
            public int ShopItemsBought { get; set; }
        }

        public class MetricDTO
        {
            public MetricTypes Id { get; set; }
            public float AverageValue { get; set; }
            public float MaxValue { get; set; }
        }

        public class NodeDTO
        {
            public string Name { get; set; }
            public IEnumerable<MetricDTO> Metrics { get; set; }

            public class MetricDTO
            {
                public MetricTypes Id { get; set; }
                public float Value { get; set; }
            }
        }
    }
}