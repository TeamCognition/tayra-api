using System.Collections.Generic;
using Tayra.Analytics;
using Tayra.Common;

namespace Tayra.Services
{
    public class TeamSwarmPlotDTO
    {
        public int LastUpdateDateId { get; set; }
        public DataDTO[] Metrics { get; set; }
        public class DataDTO
        {
            public MetricTypes MetricTypes { get; set; }
            public float[] Averages { get; set; }
            public Dictionary<int, float[]> ProfileStats { get; set; }    
        }   	
    }
        // public Dictionary<int, MetricsValueWEntity[]> ProfileMetrics { get; set; }
        // public Dictionary<int, MetricValue> Averages { get; set; }
}