namespace Tayra.Analytics
{
    public class MetricShard //MetricValueShard
    {
        public MetricType Type { get; set; }
        public float Value { get; set; }
        public int DateId { get; set; }

        public MetricShard()
        {
        }

        public MetricShard(float value, int dateId, MetricType metricType)
        {
            this.Value = value;
            this.DateId = dateId;
            this.Type = metricType;
        }
    }
}