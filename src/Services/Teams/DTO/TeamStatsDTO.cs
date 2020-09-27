using System;
using Tayra.Analytics;
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

            public OtherTeamsAveragesDTO[] TeamsAverages { get; set; }
            public float[] WeeklyAverages {get; set;}
            
            public class OtherTeamsAveragesDTO
            {
                public int Id { get; set; }
                public float[] Averages { get; set; }
                public float? TotalAverage { get; set; }
            } 
        }
    }
}