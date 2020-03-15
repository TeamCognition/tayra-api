using Firdaws.Core;
using Microsoft.AspNetCore.Mvc;
using System;
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

        #region Properties
        protected OrganizationDbContext DbContext { get; set; }

        #endregion

        #endregion

        protected OrganizationDbContext OrganizationContext;

        #region Action Methods

        [HttpGet("overview")]
        public ActionResult<AdvisorOverviewDTO> GetActionPointOverview()
        {
            return Ok(AdvisorService.GetActionPointOverview());
        }

        [HttpPost, Route("{segmentId}/segmentView")]
        public ActionResult<AdvisorSegmentViewDTO> GetSegmentView([FromRoute] int segmentId)
        {
            return Ok(AdvisorService.GetSegmentView(segmentId));
        }

        [HttpPost, Route("{segmentId}/segmentActionPoints")]
        public ActionResult<GridData<AdvisorSegmentGridDTO>> GetSegmentActionPointGrid(GridParams gridParams,[FromRoute] int segmentId)
        {
            return Ok(AdvisorService.GetSegmentActionPointGrid(gridParams,segmentId));
        }

        [HttpPut("conclude")]
        public IActionResult UpdateTeam([FromBody]AdvisorConcludeActionPointDTO dto)
        {
            AdvisorService.Conclude(dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}
