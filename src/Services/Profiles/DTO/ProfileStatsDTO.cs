using System;
using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{   
    public class ProfileStatsDTO
    {
        public DateTime? LastRefreshAt { get; set; }
        public Dictionary<int, AnalyticsMetricWithIterationSplitDto> ProfileMetrics { get; set; }
        public Dictionary<int, AnalyticsMetricWithIterationSplitDto> AssignmentMetrics { get; set; }
    }
}