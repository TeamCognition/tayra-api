namespace Tayra.Common
{
    public abstract class PureMetric
    {
        public MetricType Type { get; }
        public int DateId { get; }
        public float Value { get; }
        
        protected PureMetric(MetricType type, float value, int dateId)
        {
            this.Type = type;
            this.DateId = dateId;
            this.Value = value;
        }
    }
}