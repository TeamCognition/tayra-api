using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.SyncServices.Metrics
{
    public abstract class Metric : ITimeStampedEntity
    {
        public MetricTypes Type { get; }
        public int DateId { get; protected set; }
        public float Value { get; protected set; }

        public Metric(MetricTypes type)
        {
            this.Type = type;
        }
        
        #region ITimeStampedEntity
        
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        
        #endregion
    }
}