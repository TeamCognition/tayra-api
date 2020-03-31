using System;
using Microsoft.AspNetCore.Mvc;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class ReportsController : BaseController
    {
        #region Constructor

        public ReportsController(IServiceProvider serviceProvider, OrganizationDbContext context) : base(serviceProvider)
        {
            DbContext = context;
        }

        #endregion

        public OrganizationDbContext DbContext { get; set; }

        #region Action Methods

        [HttpGet("statuses")]
        public ActionResult<ReportStatusDTO[]> GetReportStatuses()
        {
            return ReportsService.GetReportStatus(CurrentUser.SegmentsIds);
        }

        [HttpPost("unlock/{segmentId:int}")]
        public IActionResult UnlockReporting(int segmentId)
        {
            ReportsService.UnlockReporting(segmentId);
            DbContext.SaveChanges();

            return Ok();
        }

        [HttpGet("overview")]
        public ActionResult<ReportOverviewDTO> GetOverviewReport([FromQuery] ReportParams reportParams)
        {
            return ReportsService.GetOverviewReport(reportParams);
        }

        [HttpGet("overview/membersPerformance")]
        public ActionResult<ReportMembersPerformanceDTO> GetMembersPerformanceReport([FromQuery] ReportParams reportParams)
        {
            return ReportsService.GetMembersPerformanceReport(reportParams);
        }

        [HttpGet("delivery/segmentMetrics")]
        public ActionResult<ReportDeliverySegmentMetricsDTO> GetDeliverySegmentMetrics([FromQuery] ReportParams reportParams)
        {
            return ReportsService.GetDeliverySegmentMetricsReport(reportParams);
        }

        [HttpGet("delivery/teamMetrics")]
        public ActionResult<ReportDeliveryTeamMetricsDTO> GetDeliveryTeamMetrics([FromQuery]int teamId, [FromQuery] ReportParams reportParams)
        {
            return ReportsService.GetDeliveryTeamMetricsReport(teamId, reportParams);
        }

        [HttpGet("statistics/segmentMetrics")]
        public ActionResult<ReportStatisticsSegmentMetricsDTO> GetStatisticsSegmentMetrics([FromQuery] ReportParams reportParams)
        {
            return ReportsService.GetStatisticsSegmentMetricsReport(reportParams);
        }

        [HttpGet("statistics/teamMetrics")]
        public ActionResult<ReportStatisticsTeamMetricsDTO> GetStatisticsSegmentMetrics([FromQuery]int teamId, [FromQuery] ReportParams reportParams)
        {
            return ReportsService.GetStatisticsTeamMetricsReport(teamId, reportParams);
        }

        [HttpGet("tokens/segmentMetrics")]
        public ActionResult<ReportTokensSegmentMetricsDTO> GetTokensSegmentMetrics([FromQuery] ReportParams reportParams)
        {
            return ReportsService.GetTokensSegmentMetricsReport(reportParams);
        }

        [HttpGet("tokens/teamMetrics")]
        public ActionResult<ReportTokensTeamMetricsDTO> GetTokensTeamMetrics([FromQuery] ReportParams reportParams)
        {
            return ReportsService.GetTokensTeamMetricsReport(ReportAggregationMethods.Average, ReportTimeIntervals.Month, reportParams);
        }

        [HttpGet("items/segmentMetrics")]
        public ActionResult<ReportItemsSegmentMetricsDTO> GetItemsSegmentMetrics([FromQuery] ReportParams reportParams)
        {
            return ReportsService.GetItemsSegmentMetricsReport(reportParams);
        }

        [HttpGet("items/teamMetrics")]
        public ActionResult<ReportItemsTeamMetricsDTO> GetItemsTeamMetrics([FromQuery]int teamId, [FromQuery] ReportParams reportParams)
        {
            return ReportsService.GetItemTeamMetricsReport(teamId, reportParams);
        }

        #endregion
    }
}
