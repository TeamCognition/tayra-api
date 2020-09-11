using System.Linq;
using Cog.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public class AnalyticsMetricWithIterationSplitDto
    {
        public DatePeriod Period { get; set; }
        public float Value { get; set; }
        public IterationDto[] Iterations { get; set; }
    
        public AnalyticsMetricWithIterationSplitDto(MetricType metricType, DatePeriod period, MetricRaw[] raws, EntityTypes entityType, int profilesCount)
        {
            this.Period = period;
            this.Value = metricType.Calc(raws, period);
            this.Iterations = period.SplitToIterations().Select(i => new IterationDto(metricType, i, raws, entityType, profilesCount)).ToArray();
        }
        
        public class IterationDto
        {
            public DatePeriod Period { get; set; }
            public float Value { get; set; }
            
            public IterationDto(MetricType type, DatePeriod iterationPeriod, MetricRaw[] raws, EntityTypes entityType, int profilesCount)
            {
                if (entityType == EntityTypes.Profile || profilesCount == 0)
                    profilesCount = 1;
                Period = iterationPeriod;
                Value = type.Calc(raws, iterationPeriod) / profilesCount;
            }
        }
    }
}