using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public class AnalyticsFilterRowDTO
    {
        public List<AnalyticsEntity> MetricsRows { get; set; }

        public class AnalyticsEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public Metric[] MetricsValues { get; set; }

            public class Metric
            {
                public MetricTypes Id { get; set; }
                public float Averages { get; set; }
            }
        }
    }
}