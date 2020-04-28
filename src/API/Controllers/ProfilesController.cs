using System;
using System.Collections.Generic;
using Firdaws.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
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
            return Ok(ProfilesService.GetProfileViewDTO(CurrentUser.ProfileId, x => x.Id == CurrentUser.ProfileId));
        }

        [HttpGet("{username}")]
        public ActionResult<ProfileViewDTO> GetCurrentUser([FromRoute] string username)
        {
            return Ok(ProfilesService.GetProfileViewDTO(CurrentUser.ProfileId, x => x.Username == username));
        }

        [HttpPost("search")]
        public ActionResult<GridData<ProfileGridDTO>> Search([FromBody] ProfileGridParams gridParams)
        {
            return ProfilesService.GetGridData(CurrentUser.ProfileId, gridParams);
        }

        [HttpPost("searchWithSummary")]
        public ActionResult<GridData<ProfileSummaryGridDTO>> SearchWithSummary([FromBody] ProfileSummaryGridParams gridParams)
        {
            return ProfilesService.GetGridDataWithSummary(CurrentUser.ProfileId, gridParams);
        }

        [HttpPost("searchCompletedChallenges")]
        public ActionResult<GridData<ProfileCompletedChallengesGridDTO>> GetCompletedChallengesGrid([FromBody] ProfileCompletedChallengesGridParams gridParams)
        {
            //gridParams.ProfileUsernameQuery ??= CurrentUser.Username
            gridParams.ProfileId = gridParams.ProfileId ?? CurrentUser.ProfileId;
            return ProfilesService.GetCompletedChallengesGridDTO(gridParams);
        }

        [HttpPost("searchCommittedChallenges")]
        public ActionResult<GridData<ProfileCommittedChallengesGridDTO>> GetCommittedChallengesGrid([FromBody] ProfileCommittedChallengesGridParams gridParams)
        {
            //gridParams.ProfileUsernameQuery ??= CurrentUser.Username
            gridParams.ProfileId = gridParams.ProfileId ?? CurrentUser.ProfileId;
            return ProfilesService.GetCommittedChallengesGridDTO(gridParams);
        }

        [HttpGet, Route("integrations")]
        public ActionResult<List<IntegrationProfileConfigDTO>> GetProfileIntegrations()
        {
            return IntegrationsService.GetProfileIntegrationsWithPending(CurrentUser.SegmentsIds, CurrentUser.ProfileId);
        }

        [HttpGet("update")]
        public ActionResult<ProfileUpdateDTO> GetUpdateProfile()
        {
            return ProfilesService.GetUpdateProfileData(CurrentUser.ProfileId);
        }

        [HttpPut("update")]
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

        public interface IPraisesProfile //for swagger
        {
            int TotalUps { get; set; }
        }

        [HttpPost("praise")]
        public ActionResult<IPraisesProfile> PraiseProfile([FromBody] ProfilePraiseDTO dto)
        {
            int totalUps = ProfilesService.PraiseProfile(CurrentUser.ProfileId, dto);
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

        [HttpGet("notification/settings")]
        public ActionResult<ProfileNotificationSettingsDTO> GetNotificationSettings()
        {
            return ProfilesService.GetNotificationSettings(CurrentUser.ProfileId);

        }

        [HttpPut("notification/settings")]
        public IActionResult UpdateNotificationSettings([FromBody] ProfileNotificationSettingsDTO dto)
        {
            ProfilesService.UpdateNotificationSettings(CurrentUser.ProfileId, dto);
            DbContext.SaveChanges();

            return Ok();
        }

        [HttpGet("activityChart/{profileId:int}")]
        public ActionResult<ProfileActivityChartDTO[]> GetActivityChart(int profileId)
        {
            return ProfilesService.GetProfileActivityChart(profileId);
        }

        #endregion
    }
}