using System;
using System.Linq;
using Cog.Core;
using Tayra.Analytics;
using Tayra.Common;

namespace Tayra.Services
{
    public class MetricsValueWEntity : MetricValue
    {
        public Guid EntityId { get; set; }

        public static MetricsValueWEntity Create(MetricType metricType, Guid entityId, DatePeriod period, MetricShardWEntity[] rawsWEntity, EntityTypes entityType)
        {
            var raws = rawsWEntity.Where(m => m.EntityId == entityId).ToArray();
            var value = entityType == EntityTypes.Profile ? metricType.Calc(raws, period) : metricType.CalcGroup(raws, period);
            return new MetricsValueWEntity(metricType, period, value, entityId);
        }

        public MetricsValueWEntity(MetricType type, DatePeriod period, float value, Guid entityId) : base(type, period, value)
        {
            this.EntityId = entityId;
        }
    }
}