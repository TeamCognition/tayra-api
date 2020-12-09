using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
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

        [HttpGet("{username}/rawScore")]
        public ActionResult<ProfileViewDTO> GetUserRawScore([FromRoute] string username)
        {
            return Ok(ProfilesService.GetProfileRawScoreDTO(username));
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

        [HttpPost("searchCompletedQuests")]
        public ActionResult<GridData<ProfileCompletedQuestsGridDTO>> GetCompletedQuestsGrid([FromBody] ProfileCompletedQuestsGridParams gridParams)
        {
            //gridParams.ProfileUsernameQuery ??= CurrentUser.Username
            gridParams.ProfileId = gridParams.ProfileId ?? CurrentUser.ProfileId;
            return ProfilesService.GetCompletedQuestsGridDTO(gridParams);
        }

        [HttpPost("searchCommittedQuests")]
        public ActionResult<GridData<ProfileCommittedQuestsGridDTO>> GetCommittedQuestsGrid([FromBody] ProfileCommittedQuestsGridParams gridParams)
        {
            //gridParams.ProfileUsernameQuery ??= CurrentUser.Username
            gridParams.ProfileId = gridParams.ProfileId ?? CurrentUser.ProfileId;
            return ProfilesService.GetCommittedQuestsGridDTO(gridParams);
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

        [HttpPut("togglePersonalAnalytics")]
        public IActionResult TogglePersonalAnalytics()
        {
            ProfilesService.TogglePersonalAnalytics(CurrentUser.ProfileId);
            DbContext.SaveChanges();
            return Ok();
        }

        [AllowAnonymous, HttpGet("isUsernameUnique")]
        public ActionResult<bool> IsUsernameUnique([FromQuery] string username)
        {
            return ProfilesService.IsUsernameUnique(username);
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
        public ActionResult<ProfileActivityChartDTO[]> GetActivityChart(Guid profileId)
        {
            return ProfilesService.GetProfileActivityChart(profileId);
        }

        [HttpGet("statsWidget/{profileId:int}")]
        public ActionResult<ProfileStatsDTO> GetProfileStatsData(Guid profileId)
        {
            return ProfilesService.GetProfileStatsData(profileId);
        }

        [HttpGet("heatStream/{profileId:int}")]
        public ActionResult<Dictionary<int, AnalyticsMetricWithIterationSplitDto>> GetProfileHeatStream(Guid profileId)
        {
            return ProfilesService.GetProfileHeatStream(profileId);
        }
        
        [HttpGet("sessionCache")]
        public ProfileSessionCacheDTO GetSessionCache()
        {
            var cache = (from p in DbContext.Profiles.Where(x => x.IdentityId == CurrentUser.IdentityId)
                select new ProfileSessionCacheDTO
                {
                    ProfileId = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Username = p.Username,
                    Role = p.Role,
                    Avatar = p.Avatar,
                    IsAnalyticsEnabled = p.IsAnalyticsEnabled
                }).FirstOrDefault();

            (IQueryable<Segment> qs, IQueryable<Team> qt) = Auth.Controllers.AuthorizationController.GetSegmentAndTeamQueries(DbContext, cache.ProfileId, cache.Role);

            cache.Segments = qs.Select(s => new ProfileSessionCacheDTO.SegmentDTO
            {
                Id = s.Id,
                Key = s.Key,
                Name = s.Name,
                Avatar = s.Avatar
            }).ToArray();

            cache.Teams = qt.Select(t => new ProfileSessionCacheDTO.TeamDTO
            {
                Id = t.Id,
                Key = t.Key,
                Name = t.Name,
                AvatarColor = t.AvatarColor,
                SegmentId = t.SegmentId
            }).ToArray().Where(x => x.Key != null).ToArray();

            var activeItems = Services.ProfilesService.GetProfileActiveItems(DbContext, cache.ProfileId);

            cache.Title = activeItems.Title;
            cache.Badges = activeItems.Badges;
            cache.Border = activeItems.Border;

            cache.TenantHost = CurrentUser.CurrentTenantKey;
            return cache;
        }

        #endregion
    }
}