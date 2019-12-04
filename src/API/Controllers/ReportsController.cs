using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("projectPerformance")]
        public ActionResult<ReportProjectPerformanceChartDTO> GetProjectPerformanceReport()
        {
            var report = ReportsService.GetReportProjectPerformanceChartDTO(CurrentProject.Id);
            return Ok(report);
        }

        [HttpGet("teamPerformance/{teamId}")]
        public ActionResult<ReportTeamsPerformanceChartDTO> GetTeamPerformanceReport([FromRoute]int teamId)
        {
            var report = ReportsService.GetReportTeamPerformanceChartDTO(teamId, 7);
            return Ok(report);
        }

        [HttpGet("teamsPerformance")]
        public ActionResult<ReportTeamsPerformanceChartDTO> GetTeamsPerformanceReport()
        {
            var report = ReportsService.GetReportTeamsPerformanceChartDTO(CurrentProject.Id, 7);
            return Ok(report);
        }

        [HttpGet("teamsCompletedTasks")]
        public ActionResult<IList<ReportTeamsPerformanceChartDTO>> GetTeamsCompletedTasksReport()
        {
            var report = ReportsService.GetReportTeamsCompletedTasksChartDTO(CurrentProject.Id);
            return Ok(report);
        }

        #endregion
    }
}
