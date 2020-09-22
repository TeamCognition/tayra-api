namespace Tayra.Common
{
    public abstract class PureMetricWithSegment : PureMetric
    {
        public int SegmentId { get; }

        protected PureMetricWithSegment(MetricType type, float value, int dateId, int segmentId) : base(type, value, dateId)
        {
            this.SegmentId = segmentId;
        }
    }
}