namespace Tayra.Services
{
    public interface IReportsService
    {
        ReportOverviewDTO GetOverviewReport(ReportParams reportParams);

        ReportDeliverySegmentMetricsDTO GetDeliverySegmentMetricsReport(ReportParams reportParams);
        ReportDeliveryTeamMetricsDTO GetDeliveryTeamMetricsReport(int teamId, ReportParams reportParams);

        ReportStatisticsSegmentMetricsDTO GetStatisticsSegmentMetricsReport(ReportParams reportParams);
        ReportStatisticsTeamMetricsDTO GetStatisticsTeamMetricsReport(int teamId, ReportParams reportParams);
    }
}
