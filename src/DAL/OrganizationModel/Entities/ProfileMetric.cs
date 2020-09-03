using System;
using System.Linq;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ProfileMetric: ITimeStampedEntity
    {
        public int ProfileId { get; private set; }
        public Profile Profile { get; set; }
        
        public int DateId { get; private set; }
        
        public MetricType Type { get; private set; }
        
        public int? SegmentId { get; private set; }
        public Segment Segment { get; set; }
        
        public float Value { get; set; }

        protected ProfileMetric(){}

        public ProfileMetric(int profileId, Metric metric)
        {
            ProfileId = profileId;
            DateId = metric.DateId;
            Type = metric.Type;
            Value = metric.Value;
        }

        public ProfileMetric(int profileId, SegmentMetric metric)
        {
            ProfileId = profileId;
            SegmentId = metric.SegmentId;
            DateId = metric.DateId;
            Type = metric.Type;
            Value = metric.Value;
        }

        public static ProfileMetric[] CreateRange(int profileId, SegmentMetric[] metric)
        {
            return metric.Select(x => new ProfileMetric(profileId, x)).ToArray();
        }
        
        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}