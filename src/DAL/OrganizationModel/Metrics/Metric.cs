using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
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
    }
}