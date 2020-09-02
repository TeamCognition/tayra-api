using System;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;

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
        public IActionResult GetActionPointOverview()
        {
            return Ok(AnalyticsService.GetAnalytics(2, 20200801, 20200901));
        }

        #endregion
    }
}
