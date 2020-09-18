using System.Collections.Generic;
using Cog.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public interface IAnalyticsService
    {
        Dictionary<int, AnalyticsMetricDto> GetMetrics(MetricType[] metricTypes, int entityId, EntityTypes entityType, DatePeriod period);

        Dictionary<int, AnalyticsMetricWithIterationSplitDto> GetMetricsWithIterationSplit(MetricType[] metricTypes,
            int entityId, EntityTypes entityType, DatePeriod period);
        Dictionary<int, AnalyticsMetricWithBreakdownDto> GetMetricsWithBreakdown(int entityId, EntityTypes entityType, DatePeriod period);

        Dictionary<int, AnalyticsMetricsWEntityDto[]> GetMetricsRanks(MetricType[] metricTypes, int[] entityIds,
            EntityTypes entityType, DatePeriod period);
    }
}