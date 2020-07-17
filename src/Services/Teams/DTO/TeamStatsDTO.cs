using System;
using Tayra.Common;

namespace Tayra.Services
{   
    public class TeamStatsDTO
    {
        public int LatestUpdateDateId;
        public TeamMetricDTO[] Metrics;
        
        public class TeamMetricDTO
        {
            public MetricTypes Id {get; set;}
            public float[] WeeklyAverages {get; set;}
        }
    }
}