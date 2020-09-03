using System.Linq;
using Cog.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public class AnalyticsMetricDto
    {
        public DatePeriod Period { get; set; }
        public IterationBreakdownDto.MetricDto Metric { get; set; }
        public IterationBreakdownDto[] IterationsBreakdown { get; set; }
    
        public AnalyticsMetricDto(MetricType metricType, DatePeriod period, MetricRaw[] raws)
        {
            this.Metric = new IterationBreakdownDto.MetricDto
            {
                Type = metricType,
                Value = metricType.Calc(raws, period)
            };
            this.Period = period;
            this.IterationsBreakdown = period.SplitToIterations().Select(i => new IterationBreakdownDto(metricType.BuildingMetrics, i, raws)).ToArray();
        }
        
        public class IterationBreakdownDto
        {
            public DatePeriod Period { get; set; }
            public MetricDto[] Metrics { get; set; }

            public IterationBreakdownDto(MetricType[] types, DatePeriod iterationPeriod, MetricRaw[] raws)
            {
                Period = iterationPeriod;
                Metrics = types.Select(t => new MetricDto()
                {
                    Type = t,
                    Value = t.Calc(raws, iterationPeriod)
                }).ToArray();
            }
            public class MetricDto
            {
                public MetricType Type { get; set; }
                public float Value { get; set; }   
            }
        }
    }
}