using Cog.Core;

namespace Tayra.Services
{
    public interface IAnalyticsService
    {
        public AnalyticsFilterRowDTO GetAnalyticsFilterRows(FilterRowBodyDTO body);
        AnalyticsMetricDto[] GetAnalyticsWithBreakdown(int entityId, string entityType, DatePeriod period);
    }
}