using System;
using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public class ProfileStatsDTO
    {
        public DateTime? LastRefreshAt { get; set; }
        public Dictionary<int, AnalyticsMetricWithIterationSplitDto> EntityMetrics { get; set; }
        public Dictionary<int, AnalyticsMetricWithIterationSplitDto> ComparatorMetrics { get; set; }
    }
}