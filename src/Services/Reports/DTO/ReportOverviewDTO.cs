using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public class ReportOverviewDTO
    {
        public MetricDTO[] Metrics { get;set; }
        public NodeDTO[] Nodes { get; set; }

        public class MetricDTO
        {
            public MetricTypes Id { get; set; }
            public float AverageValue { get; set; }
            public float MaxValue { get; set; }
        }

        public class NodeDTO
        {
            public string Name { get; set; }
            public IEnumerable<DataDTO> Data { get; set; }

            public class DataDTO
            {
                public MetricTypes MetricId { get; set; }
                public float Value { get; set; }
            }
        }
    }
}