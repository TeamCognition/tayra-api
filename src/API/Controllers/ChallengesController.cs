using System;
using Cog.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class ChallengesController : BaseController
    {
        #region Constructor

        public ChallengesController(IServiceProvider serviceProvider, OrganizationDbContext context) : base(serviceProvider)
        {
            OrganizationContext = context;
        }

        #endregion

        public OrganizationDbContext OrganizationContext { get; set; }

        #region Action Methods

        [HttpPost("search")]
        public ActionResult<GridData<ChallengeViewGridDTO>> GetChallengesGrid([FromBody] ChallengeViewGridParams gridParams)
        {
            return ChallengesService.GetChallengesGrid(CurrentUser.SegmentsIds, gridParams);
        }

        [HttpPost("searchCommits")]
        public ActionResult<GridData<ChallengeCommitsGridDTO>> GetChallengeCommits([FromBody] ChallengeCommitsGridParams gridParams)
        {
            return ChallengesService.GetChallengeCommitsGrid(CurrentUser.ProfileId, gridParams);
        }

        [HttpPost("searchCompletitions")]
        public ActionResult<GridData<ChallengeCompletitionsGridDTO>> GetChallengeCompletitions([FromBody] ChallengeCompletitionsGridParams gridParams)
        {
            return ChallengesService.GetChallengeCompletitionsGrid(CurrentUser.ProfileId, gridParams);
        }

        [HttpGet("{challengeId:int}")]
        public ActionResult<ChallengeViewDTO> GetSegmentChallenges([FromRoute] int challengeId)
        {
            return ChallengesService.GetChallengeViewDTO(CurrentUser.ProfileId, challengeId);
        }

        [HttpPost("create")]
        public IActionResult CreateChallenge([FromBody] ChallengeCreateDTO dto)
        {
            ChallengesService.Create(dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPut("update/{challengeId:int}")]
        public IActionResult UpdateChallenge([FromRoute]int challengeId, [FromBody] ChallengeUpdateDTO dto)
        {
            ChallengesService.Update(challengeId, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("completeGoal")]
        public IActionResult CompleteGoal([FromBody] ChallengeGoalCompleteDTO dto)
        {
            ChallengesService.CompleteGoal(CurrentUser.ProfileId, dto);
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

        [HttpPost("commit")]
        public IActionResult CommitToChallenge([FromBody] ChallengeCommitDTO dto)
        {
            ChallengesService.CommitToChallenge(CurrentUser.ProfileId, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("end/{challengeId}")]
        public IActionResult EndChallenge([FromRoute]int challengeId)
        {
            ChallengesService.EndChallenge(CurrentUser.ProfileId, challengeId);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}
