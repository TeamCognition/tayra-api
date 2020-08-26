using System;
using Tayra.Common;

namespace Tayra.Services
{   
    public class ProfileStatsDTO
    {
        public int LatestUpdateDateId;
        public ProfileMetricDTO[] Metrics;
        
        public class ProfileMetricDTO
        {
            public MetricTypes Id {get; set;}
            public AssignmentAveragesDTO[] SegmentsAverages {get; set;}
            public AssignmentAveragesDTO[] TeamsAverages {get; set;}
            public float[] WeeklyAverages {get; set;}

            public class AssignmentAveragesDTO
            {
                public int Id { get; set; }
                public float[] Averages { get; set; }
                public float? TotalAverage { get; set; }
            } 
        }
    }
}