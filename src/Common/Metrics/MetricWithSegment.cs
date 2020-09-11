namespace Tayra.Common
{
    public abstract class MetricWithSegment : Metric
    {
        public int SegmentId { get; }

        protected MetricWithSegment(MetricType type, int dateId, int segmentId) : base(type, dateId)
        {
            this.SegmentId = segmentId;
        }
    }
}