using System;
using System.Collections.Generic;
using Tayra.Analytics;
using Tayra.Common;

namespace Tayra.Services
{
    public class TeamStatsDTO
    {
        public DateTime? LastRefreshAt { get; set; }
        public Dictionary<int, AnalyticsMetricWithIterationSplitDto> EntityMetrics { get; set; }
        public Dictionary<int, AnalyticsMetricWithIterationSplitDto> ComparatorMetrics { get; set; }
    }
}