using System;
using Firdaws.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Common;
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

        #region Properties

        protected OrganizationDbContext OrganizationContext;

        #endregion

        #region Action Methods

        [HttpGet("overview")]
        public ActionResult<AdvisorOverviewDTO> GetActionPointOverview([FromQuery]int? segmentId)
        {
            return Ok(AdvisorService.GetActionPointOverview(segmentId));
        }

        //planned for future feature
        [HttpPost("searchMemberActionPoints/{profileId:int}")]
        public ActionResult<GridData<AdvisorMemberGridDTO>> GetMemberActionPointGrid([FromRoute] int profileId, GridParams gridParams)
        {
            return AdvisorService.GetMemberActionPointGrid(gridParams, profileId);
        }

        [HttpPost("searchSegmentActionPoints/{segmentId:int}")]
        public ActionResult<GridData<AdvisorSegmentGridDTO>> GetSegmentActionPointGrid([FromRoute] int segmentId, GridParams gridParams)
        {
            return AdvisorService.GetSegmentActionPointGrid(gridParams, segmentId);
        }

        [HttpPut("concludeActionPoints/{segmentId:int}")]
        public IActionResult ConcludeActionPoints([FromRoute]int segmentId, [FromQuery] int[] apIds, [FromQuery] ActionPointTypes? apType)
        {
            AdvisorService.ConcludeActionPoints(segmentId, apIds, apType);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}
