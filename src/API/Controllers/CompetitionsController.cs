using System;
using System.Collections.Generic;
using Firdaws.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class CompetitionsController : BaseDataController
    {
        #region Constructor

        public CompetitionsController(IServiceProvider serviceProvider, OrganizationDbContext context) : base(serviceProvider, context)
        {
        }

        #endregion

        #region Action Methods

        [HttpPost("search")]
        public ActionResult<GridData<CompetitionViewGridDTO>> GetProjectCompetitions([FromBody] CompetitionViewGridParams gridParams)
        {
            if (string.IsNullOrEmpty(gridParams.Sidx))
            {
                gridParams.Sidx = nameof(CompetitionViewGridDTO.EndedAt);
                gridParams.Sord = "DESC";
            }

            return CompetitionsService.GetProjectCompetitionsGrid(CurrentProject.Id, gridParams);
        }

        [HttpPost("competitors")] //TODO: competitorsSearch / searchCompetitors
        public ActionResult<GridData<CompetitionGridDTO>> GetCompetitorsGrid([FromBody] CompetitionGridParams gridParams)
        {
            if (string.IsNullOrEmpty(gridParams.Sidx))
            {
                gridParams.Sidx = nameof(CompetitionGridDTO.Points);
            }

            return CompetitionsService.GetGridData(gridParams);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody]CompetitionCreateDTO dto)
        {
            CompetitionsService.Create(CurrentProject.Id, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("addCompetitors")]
        public IActionResult AddCompetitors([FromQuery]int competitionId, [FromBody]IList<CompetitionAddCompetitorDTO> dto)
        {
            CompetitionsService.AddCompetitors(competitionId, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("start")]
        public IActionResult StartCompetition([FromQuery]int competitionId, [FromBody] CompetitionStartDTO dto)
        {
            CompetitionsService.StartCompetition(competitionId, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("end")]
        public IActionResult EndCompetition([FromQuery]int competitionId)
        {
            CompetitionsService.EndCompetition(competitionId);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("stop")]
        public IActionResult StopCompetition([FromQuery]int competitionId)
        {
            CompetitionsService.StopCompetition(competitionId);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}
