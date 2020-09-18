using System.Linq;
using Cog.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public class AnalyticsMetricsWEntityDto
    {
        public int EntityId { get; set; }
        public float Value { get; set; }
        
        public AnalyticsMetricsWEntityDto(MetricType metricType, int entityId, DatePeriod period, MetricRawWEntity[] rawsWEntity, EntityTypes entityType)
        {
            var raws = rawsWEntity.Where(m => m.EntityId == entityId).Select(x => x.MetricRaw).ToArray();
            this.EntityId = entityId;
            this.Value = entityType == EntityTypes.Profile ? metricType.Calc(raws, period) : metricType.CalcGroup(raws, period);
        }
    }
}