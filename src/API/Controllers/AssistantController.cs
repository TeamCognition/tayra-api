﻿using System;
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
        public ActionResult<AssistantOverviewDTO> GetActionPointOverview([FromQuery]int? segmentId)
        {
            return Ok(AssistantService.GetActionPointOverview(segmentId));
        }
        
        [HttpPost("{profileId:int}/searchMemberActionPoints")]
        public ActionResult<GridData<AssistantMemberGridDTO>> GetMemberActionPointGrid([FromRoute] int profileId, GridParams gridParams)
        {
            return AssistantService.GetMemberActionPointGrid(gridParams, profileId);
        }

        [HttpPost("{segmentId:int}/searchSegmentActionPoints")]
        public ActionResult<GridData<AssistantSegmentGridDTO>> GetSegmentActionPointGrid([FromRoute] int segmentId, GridParams gridParams)
        {
            return AssistantService.GetSegmentActionPointGrid(gridParams, segmentId);
        }

        [HttpPut("{segmentId:int}/concludeActionPoints")]
        public IActionResult ConcludeActionPoints([FromRoute]int segmentId, [FromQuery] int[] apIds, [FromQuery] ActionPointTypes? apType)
        {
            AssistantService.ConcludeActionPoints(segmentId, apIds, apType);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}
