using System;
using System.Collections.Generic;
using Tayra.Models.Organizations.Metrics;

namespace Tayra.Services
{
    public class SegmentStatsDTO
    {
        public DateTime? LastRefreshAt { get; set; }
        public Dictionary<int, MetricService.AnalyticsMetricWithIterationSplitDto> EntityMetrics { get; set; }
        public Dictionary<int, MetricService.AnalyticsMetricWithIterationSplitDto> ComparatorMetrics { get; set; }
    }
}