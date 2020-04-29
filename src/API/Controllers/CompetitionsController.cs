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
        public ActionResult<GridData<CompetitionViewGridDTO>> GetSegmentCompetitions([FromBody] CompetitionViewGridParams gridParams)
        {
            return CompetitionsService.GetSegmentCompetitionsGrid(CurrentSegment.Id, gridParams);
        }

        [HttpPost("searchCompetitors")]
        public ActionResult<GridData<CompetitionGridDTO>> GetCompetitorsGrid([FromBody] CompetitionGridParams gridParams)
        {
            return CompetitionsService.GetGridData(gridParams);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody]CompetitionCreateDTO dto)
        {
            CompetitionsService.Create(CurrentSegment.Id, dto);
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
