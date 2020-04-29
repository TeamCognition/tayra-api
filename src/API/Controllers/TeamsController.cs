using System;
using Firdaws.Core;
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
        public ActionResult<TeamViewDTO> GetTeam([FromRoute]string teamKey)
        {
            return Ok(TeamsService.GetTeamViewDTO(teamKey));
        }

        [HttpPost("search")]
        public ActionResult<GridData<TeamViewGridDTO>> Search([FromBody] TeamViewGridParams gridParams)
        {
            return TeamsService.GetViewGridData(CurrentUser.SegmentsIds, gridParams);
        }

        [HttpPost("searchMembers")]
        public ActionResult<GridData<TeamMembersGridDTO>> GetTeamMembers([FromBody] TeamMembersGridParams gridParams)
        {
            return TeamsService.GetTeamMembersGridData(gridParams);
        }
        
        [HttpGet("chart/pieimpact")]
        public ActionResult<TeamImpactPieChartDTO> GetTeamImpactPieChart([FromQuery] int teamId)
        {     
            return TeamsService.GetImpactPieChart(teamId);
        }


        [HttpGet("chart/lineimpact")]
        public ActionResult<TeamImpactLineChartDTO> GetTeamImpactLineChart([FromQuery] int teamId)
        {
            return TeamsService.GetImpactLineChart(teamId);
        }

        [HttpPost("create")]
        public IActionResult CreateTeam([FromBody]TeamCreateDTO dto)
        {
            TeamsService.Create(dto.SegmentId, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPut("{teamId:int}/update")]
        public IActionResult UpdateTeam([FromRoute]int teamId, [FromBody] TeamUpdateDTO dto)
        {
            TeamsService.Update(teamId, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpDelete("{teamKey}")]
        public IActionResult Delete([FromRoute]string teamKey)
        {
            TeamsService.Archive(CurrentUser.ProfileId, teamKey);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}
