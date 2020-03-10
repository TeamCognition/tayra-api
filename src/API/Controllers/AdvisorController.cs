using System;
using Firdaws.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class AdvisorController : BaseController
    {
        #region Constructor
        public AdvisorController(IServiceProvider serviceProvider, OrganizationDbContext dbContext) : base(serviceProvider)
        {
            OrganizationContext = dbContext;
        }

        #endregion

        protected OrganizationDbContext OrganizationContext;

        #region Action Methods

        [HttpPost("search")]
        public ActionResult<GridData<ActionPointGridDTO>> GetActionPointGrid([FromBody] GridParams gridParams)
        {
            return Ok(AdvisorService.GetActionPointGrid(gridParams));
        }

        #endregion
    }
}
