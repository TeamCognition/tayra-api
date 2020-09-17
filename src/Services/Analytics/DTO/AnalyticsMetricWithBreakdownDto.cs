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
            this.Value = entityType == EntityTypes.Profile ? metricType.Calc(raws, period) : metricType.CalcGroup(raws, period);
            this.IterationsBreakdown = period.SplitToIterations().Select(i => new IterationBreakdownDto(metricType.BuildingMetrics.Append(metricType).ToArray(), i, raws, entityType)).ToArray();
        }
        
        public class IterationBreakdownDto
        {
            public DatePeriod Period { get; set; }
            public Dictionary<int, float> Metrics { get; set; }

            public IterationBreakdownDto(MetricType[] types, DatePeriod iterationPeriod, MetricRaw[] raws, EntityTypes entityType)
            {
                Period = iterationPeriod;
                Metrics = entityType == EntityTypes.Profile ? types.ToDictionary(t => t.Value, t => t.Calc(raws, iterationPeriod)) : types.ToDictionary(t => t.Value, t => t.CalcGroup(raws, iterationPeriod));
            }
        }
    }
}