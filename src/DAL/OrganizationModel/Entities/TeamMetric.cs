using System;
using System.Linq;
using Cog.DAL;
using Tayra.Analytics;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class TeamMetric: ITimeStampedEntity
    {
        public int TeamId { get; private set; }
        public Team Team { get; private set;}
        
        public int DateId { get; private set;}
        
        public MetricType Type { get; private set;}

        public float Value { get; private set;}

        protected TeamMetric(){}

        public TeamMetric(int teamId, int dateId, MetricType type, float value)
        {
            TeamId = teamId;
            DateId = dateId;
            Type = type;
            Value = value;
        }
        
        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}