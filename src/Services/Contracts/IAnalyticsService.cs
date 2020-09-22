using System.Collections.Generic;
using Cog.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public interface IAnalyticsService
    {
        Dictionary<int, MetricValue> GetMetrics(MetricType[] metricTypes, int entityId, EntityTypes entityType, DatePeriod period);

        Dictionary<int, AnalyticsMetricWithIterationSplitDto> GetMetricsWithIterationSplit(MetricType[] metricTypes,
            int entityId, EntityTypes entityType, DatePeriod period);
        Dictionary<int, AnalyticsMetricWithBreakdownDto> GetMetricsWithBreakdown(MetricType[] metricTypes, int entityId, EntityTypes entityType, DatePeriod period);

        Dictionary<int, MetricsValueWEntity[]> GetMetricsRanks(MetricType[] metricTypes, int[] entityIds,
            EntityTypes entityType, DatePeriod period);
    }
}