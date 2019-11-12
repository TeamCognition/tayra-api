using System;
using Firdaws.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class ChallengesController : BaseDataController
    {
        #region Constructor

        public ChallengesController(IServiceProvider serviceProvider, OrganizationDbContext context) : base(serviceProvider, context)
        {
        }

        #endregion

        #region Action Methods

        [HttpPost("search")]
        public ActionResult<GridData<ChallengeViewGridDTO>> GetProjectChallenges([FromBody] ChallengeViewGridParams gridParams)
        {
            if (string.IsNullOrEmpty(gridParams.Sidx))
            {
                gridParams.Sidx = nameof(ChallengeViewGridDTO.ActiveUntil);
                gridParams.Sord = "ASC";
            }

            return ChallengesService.GetProjectChallengesGrid(CurrentProject.Id, gridParams);
        }

        [HttpPost("create")]
        public IActionResult CreateChallenge([FromBody] ChallengeCreateDTO dto)
        {
            ChallengesService.Create(CurrentProject.Id, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPut("update")]
        public IActionResult UpdateChallenge([FromBody] ChallengeUpdateDTO dto)
        {
            ChallengesService.Update(CurrentProject.Id, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("complete")]
        public IActionResult CompleteChallenge([FromBody] ChallengeCompleteDTO dto)
        {
            ChallengesService.CompleteChallenge(dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("end/{challengeId}")]
        public IActionResult EndChallenge([FromRoute]int challengeId)
        {
            ChallengesService.EndChallenge(CurrentUser.Id, challengeId);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}
