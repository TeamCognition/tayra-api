using System;
using System.Collections.Generic;
using Firdaws.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class TeamsController : BaseDataController
    {
        #region Constructor

        public TeamsController(IServiceProvider serviceProvider, OrganizationDbContext context) : base(serviceProvider, context)
        {
        }

        #endregion

        #region Action Methods

        [HttpGet("{teamKey}")]
        public ActionResult<TeamViewDTO> GetTeam([FromRoute]string teamKey)
        {
            return Ok(TeamsService.GetTeamViewDTO(teamKey));
        }

        [HttpPost("search")]
        public ActionResult<GridData<TeamViewGridDTO>> Search([FromBody] TeamViewGridParams gridParams)
        {
            if (string.IsNullOrEmpty(gridParams.Sidx))
            {
                gridParams.Sidx = nameof(TeamViewGridDTO.Name);
                gridParams.Sord = "ASC";
            }
            
            return TeamsService.GetViewGridData(CurrentProject.Id, gridParams);
        }

        [HttpPost("members")] //TODO: searchMembers
        public ActionResult<GridData<TeamMembersGridDTO>> GetTeamMembers([FromBody] TeamMembersGridParams gridParams)
        {
            if (string.IsNullOrEmpty(gridParams.Sidx))
            {
                gridParams.Sidx = nameof(TeamMembersGridDTO.Name);
            }

            return TeamsService.GetTeamMembersGridData(gridParams);
        }

        [HttpPost("create")]
        public IActionResult CreateTeam([FromBody]TeamCreateDTO dto)
        {
            TeamsService.Create(CurrentProject.Id, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPut("update")]
        public IActionResult UpdateTeam([FromBody] TeamUpdateDTO dto)
        {
            TeamsService.Update(CurrentProject.Id, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("addMembers")]
        public IActionResult AddMembers([FromQuery]string teamKey, [FromBody]IList<TeamAddMemberDTO> dto)
        {
            TeamsService.AddMembers(teamKey, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery]string teamKey)
        {
            TeamsService.Archive(CurrentUser.Id, teamKey);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}
