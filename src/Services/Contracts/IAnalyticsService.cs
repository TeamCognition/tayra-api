using Tayra.Common;

namespace Tayra.Services
{
    public interface IAnalyticsService
    {
        MetricDto[] GetAnalytics(int profileId, int fromId, int toId);
    }
}