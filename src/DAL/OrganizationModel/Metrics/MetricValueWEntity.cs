using System.Linq;
using Cog.Core;
using Tayra.Analytics;
using Tayra.Common;

namespace Tayra.Services
{
    public class MetricsValueWEntity : MetricValue
    {
        public int EntityId { get; set; }

        public static MetricsValueWEntity Create(MetricType metricType, int entityId, DatePeriod period, MetricShardWEntity[] rawsWEntity, EntityTypes entityType)
        {
            var raws = rawsWEntity.Where(m => m.EntityId == entityId).ToArray();
            var value = entityType == EntityTypes.Profile ? metricType.Calc(raws, period) : metricType.CalcGroup(raws, period);
            return new MetricsValueWEntity(metricType, period.ToId, value, entityId);
        }

        public MetricsValueWEntity(MetricType type, int dateId, float value, int entityId) : base (type, dateId, value)
        {
            this.EntityId = entityId;
        }
    }
}