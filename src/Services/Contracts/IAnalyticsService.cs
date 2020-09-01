
namespace Tayra.Services
{
    public interface IAnalyticsService
    {
        public AnalyticsFilterRowDTO GetAnalyticsFilterRows(FilterRowBodyDTO body);
    }
}