using System;
using System.Collections.Generic;
using Firdaws.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    [ApiController]
    public class SegmentsController : BaseController
    {
        #region Constructor

        public SegmentsController(OrganizationDbContext dbContext, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            DbContext = dbContext;
        }

        #endregion

        #region Properties

        protected OrganizationDbContext DbContext { get; set; }

        #endregion

        #region Action Methods

        [HttpPost("search")]
        public ActionResult<GridData<SegmentGridDTO>> Search([FromBody] SegmentGridParams gridParams)
        {
            return SegmentsService.GetGridData(CurrentUser.ProfileId, gridParams);
        }

        [HttpPost("{segmentKey}/searchMembers")]
        public ActionResult<GridData<SegmentMemberGridDTO>> SearchMembers([FromRoute] string segmentKey, [FromBody] SegmentMemberGridParams gridParams)
        {
            return SegmentsService.GetSegmentMembersGridData(segmentKey, gridParams);
        }

        [HttpPost("{segmentKey}/searchTeams")]
        public ActionResult<GridData<SegmentTeamGridDTO>> SearchTeams([FromRoute] string segmentKey, [FromBody] SegmentTeamGridParams gridParams)
        {
            return SegmentsService.GetSegmentTeamsGridData(segmentKey, gridParams);
        }

        [HttpGet]
        public ActionResult<SegmentViewDTO> GetSegment([FromQuery] string segmentKey)
        {
            return Ok(SegmentsService.GetSegmnetViewDTO(segmentKey));
        }

        [HttpPost]
        public IActionResult Create([FromBody] SegmentCreateDTO dto)
        {
            SegmentsService.Create(CurrentUser.ProfileId, dto);
            DbContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromQuery] int segmentId, [FromBody] SegmentCreateDTO dto)
        {
            SegmentsService.Update(segmentId, dto);
            DbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public IActionResult Archive([FromQuery] int segmentId)
        {
            SegmentsService.Archive(segmentId);
            DbContext.SaveChanges();
            return Ok();
        }

        [HttpPost("{segmentKey}/addMember")]
        public IActionResult AddMember([FromRoute] string segmentKey, [FromBody] SegmentMemberAddRemoveDTO dto)
        {
            SegmentsService.AddMember(segmentKey, dto);
            DbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete("{segmentKey}/removeMember")]
        public IActionResult RemoveMember([FromRoute] string segmentKey, [FromBody] SegmentMemberAddRemoveDTO dto)
        {
            SegmentsService.RemoveMember(segmentKey, dto);
            DbContext.SaveChanges();
            return Ok();
        }

        [HttpGet, Route("{segmentKey}/integrations")]
        public ActionResult<List<IntegrationSegmentViewDTO>> GetSegmentIntegrations([FromRoute] string segmentKey)
        {
            return IntegrationsService.GetSegmentIntegrations(segmentKey);
        }

        #endregion
    }
}