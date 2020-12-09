using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cog.Core;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface IProfilesService
    {
        Profile GetByIdentityId(Guid identityId);
        Profile GetByUsername(string username);
        Profile GetByEmail(string email);
        Profile GetProfileByExternalId(string externalId, IntegrationType integrationType);
        bool IsUsernameUnique(string username);
        GridData<ProfileGridDTO> GetGridData(Guid profileId, ProfileGridParams gridParams);
        GridData<ProfileSummaryGridDTO> GetGridDataWithSummary(Guid profileId, ProfileSummaryGridParams gridParams);
        GridData<ProfileCompletedQuestsGridDTO> GetCompletedQuestsGridDTO(ProfileCompletedQuestsGridParams gridParams);
        GridData<ProfileCommittedQuestsGridDTO> GetCommittedQuestsGridDTO(ProfileCommittedQuestsGridParams gridParams);
        ProfileUpdateDTO GetUpdateProfileData(Guid profileId);
        void UpdateProfile(Guid profileId, ProfileUpdateDTO dto);
        void TogglePersonalAnalytics(Guid profileId);
        ProfileViewDTO GetProfileViewDTO(Guid profileId, Expression<Func<Profile, bool>> condition);
        ProfileRawScoreDTO GetProfileRawScoreDTO(string username);
        void ModifyTokens(ProfileRoles profileRole, ProfileModifyTokensDTO dto);
        ProfileNotificationSettingsDTO GetNotificationSettings(Guid profileId);
        void UpdateNotificationSettings(Guid profileId, ProfileNotificationSettingsDTO dto);
        ProfileActivityChartDTO[] GetProfileActivityChart(Guid profileId);
        ProfileStatsDTO GetProfileStatsData(Guid profileIdS);
        Dictionary<int, AnalyticsMetricWithIterationSplitDto> GetProfileHeatStream(Guid profileId);
    }

}