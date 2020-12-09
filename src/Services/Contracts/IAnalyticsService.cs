using System;
using System.Collections.Generic;
using Cog.Core;
using Tayra.Analytics;
using Tayra.Common;

namespace Tayra.Services
{
    public interface IAnalyticsService
    {
        Dictionary<int, MetricValue> GetMetrics(MetricType[] metricTypes, Guid entityId, EntityTypes entityType, DatePeriod period);

        Dictionary<int, AnalyticsMetricWithIterationSplitDto> GetMetricsWithIterationSplit(MetricType[] metricTypes,
            Guid entityId, EntityTypes entityType, DatePeriod period);
        Dictionary<int, AnalyticsMetricWithBreakdownDto> GetMetricsWithBreakdown(MetricType[] metricTypes, Guid entityId, EntityTypes entityType, DatePeriod period);

        Dictionary<int, MetricsValueWEntity[]> GetMetricsRanks(MetricType[] metricTypes, Guid[] entityIds,
            EntityTypes entityType, DatePeriod period);
    }
}