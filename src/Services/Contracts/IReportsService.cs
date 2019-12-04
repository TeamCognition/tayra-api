using System.Collections.Generic;

namespace Tayra.Services
{
    public interface IReportsService
    {
        ReportProjectPerformanceChartDTO GetReportProjectPerformanceChartDTO(int projectId);
        ReportTeamPerformanceChartDTO GetReportTeamPerformanceChartDTO(int teamId, int periodInDays);
        ReportTeamsPerformanceChartDTO GetReportTeamsPerformanceChartDTO(int projectId, int periodInDays);
        IList<ReportTeamsCompletedTasksChartDTO> GetReportTeamsCompletedTasksChartDTO(int projectId);
    }
}
