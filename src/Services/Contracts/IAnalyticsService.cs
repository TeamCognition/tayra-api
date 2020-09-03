using Cog.Core;

namespace Tayra.Services
{
    public interface IAnalyticsService
    {
        AnalyticsMetricDto[] GetAnalyticsWithBreakdown(int entityId, string entityType, DatePeriod period);
    }
}