using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ProfileMetrics: ITimeStampedEntity
    {
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
        
        public int SegmentId { get; set; }
        public Segment Segment { get; set; }
        public int DateId { get; set; }
        
        public MetricTypes Type { get; set; }
        
        public float Value { get; set; }

        private ProfileMetrics(){}

        public ProfileMetrics(int profileId, int segmentId, Metric metric)
        {
            ProfileId = profileId;
            DateId = metric.DateId;
            Type = metric.Type;
            Value = metric.Value;
        }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}