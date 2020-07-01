using System;
using Tayra.Common;

namespace Tayra.Services
{   
    public class ProfileStatsDTO
    {
        public int LatestUpdateDateId;
        public ProfileMetricDTO[] Metric;
        
        public class ProfileMetricDTO
        {
            public MetricTypes Id {get; set;}
            public AssignmentAveragesDTO[] SegmentAverages {get; set;}
            public AssignmentAveragesDTO[] TeamAverages {get; set;}
            public float[] WeeklyAverages {get; set;}

            public class AssignmentAveragesDTO
            {
                public int Id { get; set; }
                public float[] Averages { get; set; }
            } 
        }
    }
}