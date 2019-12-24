using System.Collections.Generic;

namespace Tayra.Services
{
    public interface IReportsService
    {
        ReportSegmentPerformanceChartDTO GetReportSegmentPerformanceChartDTO(int segmentId);
        ReportTeamPerformanceChartDTO GetReportTeamPerformanceChartDTO(int teamId, int periodInDays);
        ReportTeamsPerformanceChartDTO GetReportTeamsPerformanceChartDTO(int segmentId, int periodInDays);
        IList<ReportTeamsCompletedTasksChartDTO> GetReportTeamsCompletedTasksChartDTO(int segmentId);
    }
}
