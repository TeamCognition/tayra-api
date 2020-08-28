using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ProfileMetric: ITimeStampedEntity
    {
        public int ProfileId { get; private set; }
        public Profile Profile { get; set; }
        
        public int? SegmentId { get; private set; }
        public Segment Segment { get; set; }
        public int DateId { get; private set; }
        
        public MetricTypes Type { get; private set; }
        
        public float Value { get; set; }

        protected ProfileMetric(){}

        public ProfileMetric(int profileId, int? segmentId, Metric metric)
        {
            ProfileId = profileId;
            SegmentId = segmentId;
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