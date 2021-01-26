using System;
using Cog.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class AssistantController : BaseController
    {
        #region Constructor
        public AssistantController(IServiceProvider serviceProvider, OrganizationDbContext dbContext) : base(serviceProvider)
        {
            OrganizationContext = dbContext;
        }

        #endregion

        #region Properties

        protected OrganizationDbContext OrganizationContext;

        #endregion

        #region Action Methods

        [HttpGet("overview")]
        public ActionResult<AssistantOverviewDTO> GetActionPointOverview([FromQuery] Guid? segmentId)
        {
            return Ok(AssistantService.GetActionPointOverview(segmentId));
        }

        [HttpPost("{profileId}/searchMemberActionPoints")]
        public ActionResult<GridData<AssistantMemberGridDTO>> GetMemberActionPointGrid([FromRoute] Guid profileId, GridParams gridParams)
        {
            return AssistantService.GetMemberActionPointGrid(gridParams, profileId);
        }

        [HttpPost("{segmentId}/searchSegmentActionPoints")]
        public ActionResult<GridData<AssistantSegmentGridDTO>> GetSegmentActionPointGrid([FromRoute] Guid segmentId, GridParams gridParams)
        {
            return AssistantService.GetSegmentActionPointGrid(gridParams, segmentId);
        }

        [HttpPut("{segmentId}/concludeActionPoints")]
        public IActionResult ConcludeActionPoints([FromRoute] Guid segmentId, [FromQuery] Guid[] apIds, [FromQuery] ActionPointTypes? apType)
        {
            AssistantService.ConcludeActionPoints(segmentId, apIds, apType);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}
