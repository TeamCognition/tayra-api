using System.Collections.Generic;

namespace Tayra.Services
{
    public interface IReportsService
    {
        ReportOverviewDTO GetOverviewReport(ReportParams reportParams);
        ReportStatisticsSegmentMetricsDTO GetStatisticsSegmentMetricsReport(ReportParams reportParams);
        ReportStatisticsTeamMetricsDTO GetStatisticsTeamMetricsReport(int teamId, ReportParams reportParams);
    }
}
