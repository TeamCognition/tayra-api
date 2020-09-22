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

        public ProfileMetric(int profileId, PureMetric pureMetric)
        {
            ProfileId = profileId;
            DateId = pureMetric.DateId;
            Type = pureMetric.Type;
            Value = pureMetric.Value;
        }

        public ProfileMetric(int profileId, PureMetricWithSegment pureMetric)
        {
            ProfileId = profileId;
            SegmentId = pureMetric.SegmentId;
            DateId = pureMetric.DateId;
            Type = pureMetric.Type;
            Value = pureMetric.Value;
        }

        public static ProfileMetric[] CreateRange(int profileId, PureMetricWithSegment[] metric)
        {
            return metric.Select(x => new ProfileMetric(profileId, x)).ToArray();
        }
        
        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}