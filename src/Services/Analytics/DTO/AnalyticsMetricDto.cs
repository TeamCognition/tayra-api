using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public class AnalyticsMetricDto
    {
        public DatePeriod Period { get; set; }
        public float Value { get; set; }
        
        public AnalyticsMetricDto(MetricType metricType, DatePeriod period, MetricRaw[] raws, EntityTypes entityType)
        {
            this.Period = period;
            this.Value = entityType == EntityTypes.Profile ? metricType.Calc(raws, period) : metricType.CalcGroup(raws, period);
        }
    }
}