using System;
using System.Linq;
using Cog.DAL;
using Tayra.Analytics;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class SegmentMetric : ITimeStampedEntity
    {
        public Guid SegmentId { get; private set; }
        public Segment Segment { get; private set; }

        public int DateId { get; private set; }

        public MetricType Type { get; private set; }

        public float Value { get; private set; }

        protected SegmentMetric() { }

        public SegmentMetric(Guid segmentId, int dateId, MetricType type, float value)
        {
            SegmentId = segmentId;
            DateId = dateId;
            Type = type;
            Value = value;
        }

        // public static SegmentMetric[] CreateRange(Guid segmentId, Metric[] metric)
        // {
        //     return metric.Select(x => new SegmentMetric(segmentId, x)).ToArray();
        // }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}