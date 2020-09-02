using Tayra.Common;

namespace Tayra.Services
{
    public interface IAnalyticsService
    {
        MetricDto[] GetAnalyticsWithBreakdown(int profileId, int fromId, int toId);
    }
}