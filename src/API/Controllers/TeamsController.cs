using System;
using Cog.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class TeamsController : BaseController
    {
        #region Constructor

        public TeamsController(IServiceProvider serviceProvider, OrganizationDbContext dbContext) : base(serviceProvider)
        {
            OrganizationContext = dbContext;
        }

        #endregion

        public OrganizationDbContext OrganizationContext { get; set; }

        #region Action Methods

        [HttpGet("{segmentKey}/{teamKey}")]
        public ActionResult<TeamViewDTO> GetTeam([FromRoute] string segmentKey, [FromRoute] string teamKey)
        {
            return Ok(TeamsService.GetTeamViewDTO(segmentKey, teamKey));
        }

        [HttpGet("{teamId}/rawScore")]
        public ActionResult<TeamRawScoreDTO> GetTeamRawScore([FromRoute] Guid teamId)
        {
            return Ok(TeamsService.GetTeamRawScoreDTO(teamId));
        }

        [HttpGet("{teamId}/swarmPlot")]
        public ActionResult<TeamSwarmPlotDTO> GetTeamSwarmPlot([FromRoute] Guid teamId)
        {
            return Ok(TeamsService.GetTeamSwarmPloteDTO(teamId));
        }

        [HttpPost("search")]
        public ActionResult<GridData<TeamViewGridDTO>> Search([FromBody] TeamViewGridParams gridParams)
        {
            return TeamsService.GetViewGridData(CurrentUser.SegmentsIds, gridParams);
        }

        [HttpPost("searchProfiles")]
        public ActionResult<GridData<TeamProfilesGridDTO>> GetTeamProfiles([FromBody] TeamProfilesGridParams gridParams)
        {
            return TeamsService.GetTeamProfilesGridData(gridParams);
        }

        [HttpPost("create")]
        public IActionResult CreateTeam([FromBody] TeamCreateDTO dto)
        {
            TeamsService.Create(dto.SegmentId, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPut("{teamId:int}/update")]
        public IActionResult UpdateTeam([FromRoute] Guid teamId, [FromBody] TeamUpdateDTO dto)
        {
            TeamsService.Update(teamId, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpDelete("{teamId}")]
        public IActionResult Delete([FromRoute] Guid teamId)
        {
            TeamsService.Archive(CurrentUser.ProfileId, teamId);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpGet("statsWidget/{teamId}")]
        public ActionResult<TeamStatsDTO> GetTeamStatsData([FromRoute] Guid teamId)
        {
            return TeamsService.GetTeamStatsData(teamId);
        }

        [HttpGet("{teamId}/pulse")]
        public ActionResult<TeamPulseDTO> GetTeamPulse([FromRoute] Guid teamId)
        {
            return TeamsService.GetTeamPulse(teamId);
        }

        #endregion
    }
}
