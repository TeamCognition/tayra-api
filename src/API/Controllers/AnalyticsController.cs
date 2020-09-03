using System;
using Cog.Core;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("")]
        public ActionResult<AnalyticsMetricDto[]> GetAnalyticsWithBreakdown([FromQuery]int entityId, [FromQuery]string entityType, [FromQuery]string period)
        {
            var datePeriod = new DatePeriod(period);
            return AnalyticsService.GetAnalyticsWithBreakdown(entityId, entityType, datePeriod);
        }

        #endregion
    }
}
