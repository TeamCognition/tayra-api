using System;
using System.Linq.Expressions;
using Cog.Core;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface IProfilesService
    {
        Profile GetByIdentityId(int identityId);
        Profile GetByUsername(string username);
        Profile GetByEmail(string email);
        Profile GetProfileByExternalId(string externalId, IntegrationType integrationType);
        bool IsUsernameUnique(string username);
        GridData<ProfileGridDTO> GetGridData(int profileId, ProfileGridParams gridParams);
        GridData<ProfileSummaryGridDTO> GetGridDataWithSummary(int profileId, ProfileSummaryGridParams gridParams);
        GridData<ProfileCompletedQuestsGridDTO> GetCompletedQuestsGridDTO(ProfileCompletedQuestsGridParams gridParams);
        GridData<ProfileCommittedQuestsGridDTO> GetCommittedQuestsGridDTO(ProfileCommittedQuestsGridParams gridParams);
        ProfileUpdateDTO GetUpdateProfileData(int profileId);
        void UpdateProfile(int profileId, ProfileUpdateDTO dto);
        void TogglePersonalAnalytics(int profileId);
        ProfileViewDTO GetProfileViewDTO(int profileId, Expression<Func<Profile, bool>> condition);
        ProfileRawScoreDTO GetProfileRawScoreDTO(string username);
        void ModifyTokens(ProfileRoles profileRole, ProfileModifyTokensDTO dto);
        ProfileNotificationSettingsDTO GetNotificationSettings(int profileId);
        void UpdateNotificationSettings(int profileId, ProfileNotificationSettingsDTO dto);
        ProfileActivityChartDTO[] GetProfileActivityChart(int profileId);
        ProfileStatsDTO GetProfileStatsData(int profileIdS);
        ProfileHeatStreamDTO GetProfileHeatStream(int profileId);
    }

}