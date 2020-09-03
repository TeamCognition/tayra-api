using System;
using System.Collections.Generic;
using System.Linq;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public abstract class SegmentMetric : Metric
    {
        public int SegmentId { get; }
        
        protected SegmentMetric(MetricType type, int dateId, int segmentId) : base(type, dateId)
        {
            this.SegmentId = segmentId;
        }
    }
}