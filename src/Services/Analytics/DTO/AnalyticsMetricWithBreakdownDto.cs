using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public class AnalyticsMetricWithBreakdownDto
    {
        public DateTime? LastRefreshAt { get; set; }
        public DatePeriod Period { get; set; }
        public float Value { get; set; }
        public IterationBreakdownDto[] IterationsBreakdown { get; set; }
    
        public AnalyticsMetricWithBreakdownDto(MetricType metricType, DatePeriod period, MetricRaw[] raws, DateTime lastRefreshAt, EntityTypes entityType)
        {
            this.LastRefreshAt = lastRefreshAt;
            this.Period = period;
            if (entityType == EntityTypes.Profile)
            {
                this.Value = metricType.Calc(raws, period);
            }
            else
            {
                this.Value = raws.Where(x => x.Type == metricType).Sum(x => x.Value);
            }
            this.IterationsBreakdown = period.SplitToIterations().Select(i => new IterationBreakdownDto(metricType.BuildingMetrics.Append(metricType).ToArray(), i, raws, entityType)).ToArray();
        }
        
        public class IterationBreakdownDto
        {
            public DatePeriod Period { get; set; }
            public Dictionary<int, float> Metrics { get; set; }

            public IterationBreakdownDto(MetricType[] types, DatePeriod iterationPeriod, MetricRaw[] raws, EntityTypes entityType)
            {
                Period = iterationPeriod;
                if (entityType == EntityTypes.Profile)
                {
                    Metrics = types.ToDictionary(t => t.Value, t => t.Calc(raws, iterationPeriod));
                }
                else
                {
                    Metrics = types.ToDictionary(t => t.Value, t => raws.Where(x => x.Type == t).Sum(x => x.Value));
                }
            }
        }
    }
}