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

        [HttpGet("{teamKey}")]
        public ActionResult<TeamViewDTO> GetTeam([FromRoute] string teamKey)
        {
            return Ok(TeamsService.GetTeamViewDTO(teamKey));
        }

        [HttpGet("{teamKey}/rawScore")]
        public ActionResult<TeamRawScoreDTO> GetTeamRawScore([FromRoute] string teamKey)
        {
            return Ok(TeamsService.GetTeamRawScoreDTO(teamKey));
        }

        [HttpGet("{teamKey}/swarmPlot")]
        public ActionResult<TeamSwarmPlotDTO> GetTeamSwarmPlot([FromRoute] string teamKey)
        {
            return Ok(TeamsService.GetTeamSwarmPloteDTO(teamKey));
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

        [HttpDelete("{teamKey}")]
        public IActionResult Delete([FromRoute] string teamKey)
        {
            TeamsService.Archive(CurrentUser.ProfileId, teamKey);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpGet("statsWidget/{teamId}")]
        public ActionResult<TeamStatsDTO> GetTeamStatsData([FromRoute] Guid teamId)
        {
            return TeamsService.GetTeamStatsData(teamId);
        }

        [HttpGet("{teamKey}/pulse")]
        public ActionResult<TeamPulseDTO> GetTeamPulse([FromRoute] string teamKey)
        {
            return TeamsService.GetTeamPulse(teamKey);
        }

        #endregion
    }
}
