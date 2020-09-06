using System.Collections.Generic;
using Cog.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public interface IAnalyticsService
    {
        Dictionary<int, AnalyticsMetricDto> GetMetrics(List<MetricType> metricTypes, int entityId, string entityType, DatePeriod period);
        Dictionary<int, AnalyticsMetricDto> GetAnalyticsWithBreakdown(int entityId, string entityType, DatePeriod period);
    }
}