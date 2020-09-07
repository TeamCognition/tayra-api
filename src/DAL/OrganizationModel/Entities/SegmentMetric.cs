using System;
using System.Linq;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class SegmentMetric: ITimeStampedEntity
    {
        public int SegmentId { get; private set; }
        public Segment Segment { get; set; }
        
        public int DateId { get; private set; }
        
        public MetricType Type { get; private set; }

        public float Value { get; set; }

        protected SegmentMetric(){}

        public SegmentMetric(int segmentId, ProfileMetric profileMetric)
        {
            SegmentId = segmentId;
            DateId = profileMetric.DateId;
            Type = profileMetric.Type;
            Value = profileMetric.Value;
        }

        // public static SegmentMetric[] CreateRange(int segmentId, Metric[] metric)
        // {
        //     return metric.Select(x => new SegmentMetric(segmentId, x)).ToArray();
        // }
        
        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}