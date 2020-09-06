using System;
using System.Collections.Generic;
using Cog.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services;

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
        public ActionResult<Dictionary<int, AnalyticsMetricDto>> GetMetrics([FromQuery] List<MetricType> metricTypes, [FromQuery] int entityId, [FromQuery] string entityType, [FromQuery] string period)
        {
            var datePeriod = new DatePeriod(period);
            metricTypes = new List<MetricType>() {MetricType.Impact, MetricType.ItemsInInventory};
            return AnalyticsService.GetMetrics(metricTypes, entityId, entityType, datePeriod);
        }
        
        [HttpGet("")]
        public ActionResult<Dictionary<int, AnalyticsMetricDto>> GetAnalyticsWithBreakdown([FromQuery] int entityId, [FromQuery] string entityType, [FromQuery] string period)
        {
            var datePeriod = new DatePeriod(period);
            return AnalyticsService.GetAnalyticsWithBreakdown(entityId, entityType, datePeriod);
        }

        #endregion
    }
}
