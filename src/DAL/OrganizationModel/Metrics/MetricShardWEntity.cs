namespace Tayra.Analytics
{
    public class MetricShardWEntity //MetricValueShard
    {
        public int EntityId { get; set; }
        public MetricType Type { get; set; }
        public float Value { get; set; }
        public int DateId { get; set; }

        public MetricShardWEntity()
        {
        }

        public MetricShardWEntity(float value, int dateId, int entityId, MetricType metricType)
        {
            this.Value = value;
            this.DateId = dateId;
            this.EntityId = entityId;
            this.Type = metricType;
        }
    }
}