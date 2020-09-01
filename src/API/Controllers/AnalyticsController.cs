using System;
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
        
        [HttpPost("filterRows")]
        public ActionResult<AnalyticsFilterRowDTO> GetAnalyticsFilterRows([FromBody]FilterRowBodyDTO body)
        {
            return Ok(AnalyticsService.GetAnalyticsFilterRows(body));
        }
        
        #endregion
    }
}