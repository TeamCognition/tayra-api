using System;

namespace Tayra.Analytics
{
    public class MetricShardWEntity : MetricShard//MetricValueShard
    {
        public Guid EntityId { get; set; }

        public MetricShardWEntity()
        {
        }

        public MetricShardWEntity(float value, int dateId, Guid entityId, MetricType metricType)
        {
            this.Value = value;
            this.DateId = dateId;
            this.EntityId = entityId;
            this.Type = metricType;
        }
    }
}