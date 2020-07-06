using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public class TeamSwarmPlotDTO
    {
        public int LastUpdateDateId { get; set; }
        public DataDTO[] Metrics { get; set; }
        public class DataDTO
        {
            public MetricTypes MetricType { get; set; }
            public float[] Averages { get; set; }
            public Dictionary<int, float[]> ProfileStats { get; set; }    
        }
        
    }
}