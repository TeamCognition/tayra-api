namespace Tayra.Common
{
    public abstract class Metric
    {
        public MetricType Type { get; }
        public int DateId { get; }
        public float Value { get; protected set; }

        protected Metric(MetricType type, int dateId)
        {
            this.Type = type;
            this.DateId = dateId;
        }
        
        protected Metric(MetricType type, int dateId, float value)
        {
            this.Type = type;
            this.DateId = dateId;
            this.Value = value;
        }
    }
}