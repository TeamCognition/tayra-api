using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public abstract class Metric
    {
        public MetricTypes Type { get; }
        public int DateId { get; protected set; }
        public float Value { get; protected set; }

        protected Metric(MetricTypes type)
        {
            this.Type = type;
        }
    }
}