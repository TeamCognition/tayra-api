﻿using System;
using Firdaws.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    [ApiController]
    public class ProfilesController : BaseController
    {
        #region Constructor

        public ProfilesController(OrganizationDbContext dbContext, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            DbContext = dbContext;
        }

        #endregion

        #region Properties

        protected OrganizationDbContext DbContext { get; set; }

        #endregion

        #region Action Methods

        [HttpGet("me")]
        public ActionResult<ProfileViewDTO> GetCurrentUser()
        {
            return Ok(ProfilesService.GetProfileViewDTO(x => x.Id == CurrentUser.ProfileId));
        }

        [HttpGet("{nickname}")]
        public ActionResult<ProfileViewDTO> GetCurrentUser([FromRoute] string nickname)
        {
            return Ok(ProfilesService.GetProfileViewDTO(x => x.Nickname == nickname));
        }

        [AllowAnonymous, HttpPost("search")]
        public ActionResult<GridData<ProfileGridDTO>> Search([FromBody] ProfileGridParams gridParams)
        {
            if(string.IsNullOrEmpty(gridParams.Sidx))
            {
                gridParams.Sidx = nameof(ProfileGridDTO.Name);
                gridParams.Sord = "ASC";
            }

            return ProfilesService.GetGridData(5, gridParams);
        }

        [HttpPost("searchWithSummary")]
        public ActionResult<GridData<ProfileSummaryGridDTO>> SearchWithSummary([FromBody] ProfileSummaryGridParams gridParams)
        {
            if(string.IsNullOrEmpty(gridParams.Sidx))
            {
                gridParams.Sidx = nameof(ProfileGridDTO.Name);
                gridParams.Sord = "ASC";
            }

            return ProfilesService.GetGridDataWithSummary(CurrentUser.ProfileId, gridParams);
        }


        [HttpPost("completedChallengesSearch")]
        public ActionResult<GridData<ProfileCompletedChallengesGridDTO>> GetCompletedChallengesGrid([FromBody] ProfileCompletedChallengesGridParams gridParams)
        {
            if (string.IsNullOrEmpty(gridParams.Sidx))
            {
                gridParams.Sidx = nameof(ProfileCompletedChallengesGridDTO.CompletedAt);
                gridParams.Sord = "DESC";
            }

            //gridParams.ProfileNicknameQuery ??= CurrentUser.Nickname
            gridParams.ProfileId = gridParams.ProfileId ?? CurrentUser.ProfileId;
            return ProfilesService.GetCompletedChallengesGridDTO(gridParams);
        }

        [HttpGet("radarChart/{profileId}")]
        public ActionResult<ProfileRadarChartDTO> GetRadarChart([FromRoute] int profileId)
        {
           return Ok(ProfilesService.GetProfileRadarChartDTO(profileId));
        }

        public interface IOneUpProfile //for swagger
        {
            int TotalUps { get; set; }
        }

        [HttpPost("oneUp")]
        public ActionResult<IOneUpProfile> OneUpProfile([FromBody] ProfileOneUpDTO dto)
        {
            int totalUps = ProfilesService.OneUpProfile(CurrentUser.ProfileId, dto);
            DbContext.SaveChanges();

            return Ok(new { TotalUps = totalUps });
        }

        [HttpPost("modifyTokens")]
        public IActionResult ModifyTokens([FromBody] ProfileModifyTokensDTO dto)
        {
            ProfilesService.ModifyTokens(CurrentUser.Role, dto);
            DbContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}