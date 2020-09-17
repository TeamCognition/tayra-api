using System.Linq;
using Cog.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public class AnalyticsMetricWithIterationSplitDto
    {
        public DatePeriod Period { get; }
        public float Value { get; }
        public IterationDto[] Iterations { get; }
    
        public AnalyticsMetricWithIterationSplitDto(MetricType metricType, DatePeriod period, MetricRaw[] raws, EntityTypes entityType)
        {
            this.Period = period;
            this.Value = entityType == EntityTypes.Profile ? metricType.Calc(raws, period) : metricType.CalcGroup(raws, period);
            this.Iterations = period.SplitToIterations().Select(i => new IterationDto(metricType, i, raws, entityType)).ToArray();
        }
        
        public class IterationDto
        {
            public DatePeriod Period { get; set; }
            public float Value { get; set; }
            
            public IterationDto(MetricType metricType, DatePeriod iterationPeriod, MetricRaw[] raws, EntityTypes entityType)
            {
                Period = iterationPeriod;
                this.Value = entityType == EntityTypes.Profile ? metricType.Calc(raws, iterationPeriod) : metricType.CalcGroup(raws, iterationPeriod);
            }
        }
    }
}