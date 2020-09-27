using System.Linq;
using Cog.Core;
using Tayra.Analytics;
using Tayra.Common;

namespace Tayra.Services
{
    public class MetricsValueWEntity : MetricValue
    {
        public int EntityId { get; set; }

        public static MetricsValueWEntity Create(MetricType metricType, int entityId, DatePeriod period, MetricRawWEntity[] rawsWEntity, EntityTypes entityType)
        {
            var raws = rawsWEntity.Where(m => m.EntityId == entityId).Select(x => x.MetricShard).ToArray();
            var value = entityType == EntityTypes.Profile ? metricType.Calc(raws, period) : metricType.CalcGroup(raws, period);
            return new MetricsValueWEntity(metricType, value, period.ToId, entityId);
        }

        public MetricsValueWEntity(MetricType type, float value, int dateId, int entityId) : base (value, dateId, type)
        {
            this.EntityId = entityId;
        }
    }
}