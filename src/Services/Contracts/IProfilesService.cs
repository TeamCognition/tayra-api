using System;
using System.Linq.Expressions;
using Firdaws.Core;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface IProfilesService
    {
        Profile GetByIdentityId(int identityId);
        Profile GetByUsername(string username);
        Profile GetByEmail(string email);
        Profile GetMemberByExternalId(string externalId, IntegrationType integrationType);
        bool IsUsernameUnique(string username);
        int PraiseProfile(int profileId, ProfilePraiseDTO dto);
        GridData<ProfileGridDTO> GetGridData(int profileId, ProfileGridParams gridParams);
        GridData<ProfileSummaryGridDTO> GetGridDataWithSummary(int profileId, ProfileSummaryGridParams gridParams);
        GridData<ProfileCompletedChallengesGridDTO> GetCompletedChallengesGridDTO(ProfileCompletedChallengesGridParams gridParams);
        GridData<ProfileCommittedChallengesGridDTO> GetCommittedChallengesGridDTO(ProfileCommittedChallengesGridParams gridParams);
        ProfileUpdateDTO GetUpdateProfileData(int profileId);
        void UpdateProfile(int profileId, ProfileUpdateDTO dto);
        ProfileRadarChartDTO GetProfileRadarChartDTO(int profileId);
        ProfileViewDTO GetProfileViewDTO(int profileId, Expression<Func<Profile, bool>> condition);
        void ModifyTokens(ProfileRoles profileRole, ProfileModifyTokensDTO dto);
        ProfileNotificationSettingsDTO GetNotificationSettings(int profileId);
        void UpdateNotificationSettings(int profileId, ProfileNotificationSettingsDTO dto);
        ProfileActivityChartDTO[] GetProfileActivityChart(int profileId);
    }

}