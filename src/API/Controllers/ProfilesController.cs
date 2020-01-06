using System;
using System.Collections.Generic;
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

        [HttpGet("sessionCache")]
        public ActionResult<ProfileSessionCacheDTO> GetSessionCache()
        {
            return Ok(ProfilesService.GetSessionCache(CurrentUser.ProfileId));
        }

        [HttpGet("me")]
        public ActionResult<ProfileViewDTO> GetCurrentUser()
        {
            return Ok(ProfilesService.GetProfileViewDTO(x => x.Id == CurrentUser.ProfileId));
        }

        [HttpGet("{username}")]
        public ActionResult<ProfileViewDTO> GetCurrentUser([FromRoute] string username)
        {
            return Ok(ProfilesService.GetProfileViewDTO(x => x.Username == username));
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

            //gridParams.ProfileUsernameQuery ??= CurrentUser.Username
            gridParams.ProfileId = gridParams.ProfileId ?? CurrentUser.ProfileId;
            return ProfilesService.GetCompletedChallengesGridDTO(gridParams);
        }

        [HttpGet, Route("integrations")]
        public ActionResult<List<IntegrationProfileConfigDTO>> GetProfileIntegrations()
        {
            return IntegrationsService.GetProfileIntegrationsWithPending(CurrentUser.ProfileId);
        }

        [HttpPut]
        public IActionResult UpdateProfile([FromBody] ProfileUpdateDTO dto)
        {
            ProfilesService.UpdateProfile(CurrentUser.ProfileId, dto);
            DbContext.SaveChanges();
            return Ok();
        }

        [HttpGet("radarChart/{profileId}")]
        public ActionResult<ProfileRadarChartDTO> GetRadarChart([FromRoute] int profileId)
        {
           return Ok(ProfilesService.GetProfileRadarChartDTO(profileId));
        }

        [AllowAnonymous, HttpGet("isUsernameUnique")]
        public ActionResult<bool> IsUsernameUnique([FromQuery] string username)
        {
            return ProfilesService.IsUsernameUnique(username);
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