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

        #endregion
    }
}
