using System;
using System.Collections.Generic;

namespace Tayra.Services
{
    public class SegmentStatsDTO
    {
        public DateTime? LastRefreshAt { get; set; }
        public Dictionary<int, AnalyticsMetricWithIterationSplitDto> EntityMetrics { get; set; }
        public Dictionary<int, AnalyticsMetricWithIterationSplitDto> ComparatorMetrics { get; set; }
    }
}