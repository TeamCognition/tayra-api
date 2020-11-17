using System;
using System.Linq;
using Cog.DAL;
using Tayra.Analytics;

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

        public ProfileMetric(int profileId, MetricShard metricShard)
        {
            ProfileId = profileId;
            DateId = metricShard.DateId;
            Type = metricShard.Type;
            Value = metricShard.Value;
        }

        public ProfileMetric(int profileId, MetricShardWEntity metricShard)
        {
            ProfileId = profileId;
            SegmentId = metricShard.EntityId;
            DateId = metricShard.DateId;
            Type = metricShard.Type;
            Value = metricShard.Value;
        }

        public static ProfileMetric[] CreateRange(int profileId, MetricShardWEntity[] metric)
        {
            return metric.Select(x => new ProfileMetric(profileId, x)).ToArray();
        }
        public static ProfileMetric[] CreateRange(int profileId, MetricShard[] metric)
        {
            return metric.Select(x => new ProfileMetric(profileId, x)).ToArray();
        }
        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}