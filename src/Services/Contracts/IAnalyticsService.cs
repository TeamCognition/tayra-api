using System.Collections.Generic;
using Cog.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public interface IAnalyticsService
    {
        Dictionary<int, AnalyticsMetricDto> GetMetrics(MetricType[] metricTypes, int entityId, EntityTypes entityType, DatePeriod period);

        Dictionary<int, AnalyticsMetricWithIterationSplitDto> GetMetricsWithIterationSplit(MetricType[] metricsTypes,
            int entityId, EntityTypes entityType, DatePeriod period);
        Dictionary<int, AnalyticsMetricWithBreakdownDto> GetMetricsWithBreakdown(int entityId, EntityTypes entityType, DatePeriod period);
    }
}