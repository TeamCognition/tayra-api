﻿using System;
using Firdaws.Core;
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
            return ChallengesService.GetChallengesGrid(gridParams);
        }

        [HttpPost("searchCommits")]
        public ActionResult<GridData<ChallengeCommitGridDTO>> GetChallengeCommits([FromBody] ChallengeCommitGridParams gridParams)
        {
            return ChallengesService.GetChallengeCommitsGrid(CurrentUser.ProfileId, gridParams);
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

        [HttpPut("update")]
        public IActionResult UpdateChallenge([FromBody] ChallengeUpdateDTO dto)
        {
            ChallengesService.Update(dto);
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
