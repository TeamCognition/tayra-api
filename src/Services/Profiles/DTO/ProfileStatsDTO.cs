using System;
using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{   
    public class ProfileStatsDTO
    {
        public Dictionary<MetricType, AnalyticsMetricWithIterationSplitDto> ProfileMetrics { get; set; }
        public Dictionary<MetricType, AnalyticsMetricWithIterationSplitDto> AssignmentMetrics { get; set; }
    }
}