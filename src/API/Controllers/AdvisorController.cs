﻿using System;
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

        #region Properties

        protected OrganizationDbContext OrganizationContext;

        #endregion

        #region Action Methods

        [HttpGet("overview")]
        public ActionResult<AdvisorOverviewDTO> GetActionPointOverview([FromQuery]int? segmentId)
        {
            return Ok(AdvisorService.GetActionPointOverview(segmentId));
        }

        [HttpPost("{profileId:int}/memberActionPoints")]
        public ActionResult<GridData<AdvisorMemberGridDTO>> GetMemberActionPointGrid(GridParams gridParams, [FromRoute] int profileId)
        {
            return Ok(AdvisorService.GetMemberActionPointGrid(gridParams, profileId));
        }

        [HttpPost("{segmentId:int}/segmentActionPoints")]
        public ActionResult<GridData<AdvisorSegmentGridDTO>> GetSegmentActionPointGrid(GridParams gridParams, [FromRoute] int segmentId)
        {
            return Ok(AdvisorService.GetSegmentActionPointGrid(gridParams, segmentId));
        }

        [HttpPut("concludeSegmentActionPoints")]
        public IActionResult ConcludeSegmentActionPoints([FromBody] AdvisorSegmentConcludeDTO dto)
        {
            AdvisorService.ConcludeSegmentActionPoints(dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}
