using System;
using System.Collections.Generic;
using Cog.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.Services.Analytics;

namespace Tayra.API.Controllers
{
    public class AnalyticsController : BaseController
    {
        #region Constructor
        public AnalyticsController(IServiceProvider serviceProvider, OrganizationDbContext dbContext) : base(serviceProvider)
        {
            OrganizationContext = dbContext;
        }

        #endregion

        #region Properties

        protected OrganizationDbContext OrganizationContext;

        #endregion

        #region Action Methods
        
        [HttpGet("metrics")]
        public ActionResult<Dictionary<int, AnalyticsMetricDto>> GetMetrics([FromQuery] int entityId, [FromQuery] EntityTypes entityType, [FromQuery] string period)
        {
            var datePeriod = new DatePeriod(period);
            var metricTypes = new MetricType[] {};
            return AnalyticsService.GetMetrics(metricTypes, entityId, entityType, datePeriod);
        }
        
        [HttpGet("metricsWithBreakdown")]
        public ActionResult<Dictionary<int, AnalyticsMetricWithBreakdownDto>> GetAnalyticsWithBreakdown([FromQuery] int entityId, [FromQuery] EntityTypes entityType, [FromQuery] string period)
        {
            var datePeriod = new DatePeriod(period);
            return AnalyticsService.GetMetricsWithBreakdown(entityId, entityType, datePeriod);
        }
        
        [HttpGet("test")]
        public IActionResult GetAnalyticsWithBreakdownasd()
        {
            new AnalyticsService(OrganizationContext).Haris();
            return Ok();
        }

        #endregion
    }
}
