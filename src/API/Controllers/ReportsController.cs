using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class ReportsController : BaseDataController
    {
        #region Constructor

        public ReportsController(IServiceProvider serviceProvider, OrganizationDbContext context) : base(serviceProvider, context)
        {
        }

        #endregion

        #region Action Methods

        [HttpGet("overview")]
        public ActionResult<ReportOverviewDTO> GetOverviewReport([FromQuery] ReportParams reportParams)
        {
            return Ok(ReportsService.GetOverviewReport(reportParams));
        }

        [HttpGet("delivery/segmentMetrics")]
        public ActionResult<ReportDeliverySegmentMetricsDTO> GetDeliverySegmentMetrics([FromQuery] ReportParams reportParams)
        {
            return Ok(ReportsService.GetDeliverySegmentMetricsReport(reportParams));
        }

        [HttpGet("delivery/teamMetrics")]
        public ActionResult<ReportDeliveryTeamMetricsDTO> GetDeliveryTeamMetrics([FromQuery]int teamId, [FromQuery] ReportParams reportParams)
        {
            return Ok(ReportsService.GetDeliveryTeamMetricsReport(teamId, reportParams));
        }

        [HttpGet("statistics/segmentMetrics")]
        public ActionResult<ReportStatisticsSegmentMetricsDTO> GetStatisticsSegmentMetrics([FromQuery] ReportParams reportParams)
        {
            return Ok(ReportsService.GetStatisticsSegmentMetricsReport(reportParams));
        }

        [HttpGet("statistics/teamMetrics")]
        public ActionResult<ReportStatisticsTeamMetricsDTO> GetStatisticsSegmentMetrics([FromQuery]int teamId, [FromQuery] ReportParams reportParams)
        {
            return Ok(ReportsService.GetStatisticsTeamMetricsReport(teamId, reportParams));
        }

        [HttpGet("tokens/segmentMetrics")]
        public ActionResult<ReportTokensSegmentMetricsDTO> GetTokensSegmentMetrics([FromQuery] ReportParams reportParams)
        {
            return Ok(ReportsService.GetTokensSegmentMetricsReport(reportParams));
        }

        [HttpGet("tokens/teamMetrics")]
        public ActionResult<ReportTokensTeamMetricsDTO> GetTokensTeamMetrics([FromQuery] ReportParams reportParams)
        {
            return Ok(ReportsService.GetTokensTeamMetricsReport(ReportAggregationMethods.Average, ReportTimeIntervals.Month, reportParams));
        }

        [HttpGet("items/segmentMetrics")]
        public ActionResult<ReportItemsSegmentMetricsDTO> GetItemsSegmentMetrics([FromQuery] ReportParams reportParams)
        {
            return Ok(ReportsService.GetItemsSegmentMetricsReport(reportParams));
        }

        [HttpGet("items/teamMetrics")]
        public ActionResult<ReportItemsTeamMetricsDTO> GetItemsTeamMetrics([FromQuery]int teamId, [FromQuery] ReportParams reportParams)
        {
            return Ok(ReportsService.GetItemTeamMetricsReport(teamId, reportParams));
        }

        // [HttpGet("segmentPerformance")]
        // public ActionResult<ReportSegmentPerformanceChartDTO> GetSegmentPerformanceReport()
        // {
        //     var report = ReportsService.GetReportSegmentPerformanceChartDTO(CurrentSegment.Id);
        //     return Ok(report);
        // }

        // [HttpGet("teamPerformance/{teamId}")]
        // public ActionResult<ReportTeamsPerformanceChartDTO> GetTeamPerformanceReport([FromRoute]int teamId)
        // {
        //     var report = ReportsService.GetReportTeamPerformanceChartDTO(teamId, 7);
        //     return Ok(report);
        // }

        // [HttpGet("teamsPerformance")]
        // public ActionResult<ReportTeamsPerformanceChartDTO> GetTeamsPerformanceReport()
        // {
        //     var report = ReportsService.GetReportTeamsPerformanceChartDTO(CurrentSegment.Id, 7);
        //     return Ok(report);
        // }

        // [HttpGet("teamsCompletedTasks")]
        // public ActionResult<IList<ReportTeamsPerformanceChartDTO>> GetTeamsCompletedTasksReport()
        // {
        //     var report = ReportsService.GetReportTeamsCompletedTasksChartDTO(CurrentSegment.Id);
        //     return Ok(report);
        // }

        #endregion
    }
}
