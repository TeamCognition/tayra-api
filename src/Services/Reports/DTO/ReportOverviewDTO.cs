using System.Collections.Generic;

namespace Tayra.Services
{
    public class ReportOverviewDTO
    {
        public MetricDTO[] Metrics { get;set; }
        public NodeDTO[] Nodes { get; set; }

        public class MetricDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public float AverageValue { get; set; }
            public float MaxValue { get; set; }
        }

        public class NodeDTO
        {
            public string Name { get; set; }
            public IEnumerable<DataDTO> Data { get; set; }

            public class DataDTO
            {
                /// <summary>
                /// Category Id
                /// </summary>
                public int Id { get; set; }
                public float Value { get; set; }
            }
        }
    }
}