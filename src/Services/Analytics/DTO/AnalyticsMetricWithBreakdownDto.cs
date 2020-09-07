using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public class AnalyticsMetricWithBreakdownDto
    {
        public DatePeriod Period { get; set; }
        public float Value { get; set; }
        public IterationBreakdownDto[] IterationsBreakdown { get; set; }
    
        public AnalyticsMetricWithBreakdownDto(MetricType metricType, DatePeriod period, MetricRaw[] raws)
        {
            this.Period = period;
            this.Value = metricType.Calc(raws, period);
            this.IterationsBreakdown = period.SplitToIterations().Select(i => new IterationBreakdownDto(metricType.BuildingMetrics.Append(metricType).ToArray(), i, raws)).ToArray();
        }
        
        public class IterationBreakdownDto
        {
            public DatePeriod Period { get; set; }
            public Dictionary<int, float> Metrics { get; set; }

            public IterationBreakdownDto(MetricType[] types, DatePeriod iterationPeriod, MetricRaw[] raws)
            {
                Period = iterationPeriod;
                Metrics = types.ToDictionary(t => t.Value, t => t.Calc(raws, iterationPeriod));
            }
        }
    }
}