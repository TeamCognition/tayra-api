using System;
using System.Linq.Expressions;
using Firdaws.Core;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface IProfilesService
    {
        ProfileSessionCacheDTO GetSessionCache(int profileId);
        Profile GetByIdentityId(int identityId);
        Profile GetByUsername(string username);
        Profile GetByEmail(string email);
        Profile GetByExternalId(string externalId, IntegrationType integrationType);
        bool IsUsernameUnique(string username);
        int OneUpProfile(int profileId, ProfileOneUpDTO dto);
        GridData<ProfileGridDTO> GetGridData(int profileId, ProfileGridParams gridParams);
        GridData<ProfileSummaryGridDTO> GetGridDataWithSummary(int profileId, ProfileSummaryGridParams gridParams);
        GridData<ProfileCompletedChallengesGridDTO> GetCompletedChallengesGridDTO(ProfileCompletedChallengesGridParams gridParams);
        GridData<ProfileCommittedChallengesGridDTO> GetCommittedChallengesGridDTO(ProfileCommittedChallengesGridParams gridParams);
        ProfileUpdateDTO GetUpdateProfileData(int profileId);
        void UpdateProfile(int profileId, ProfileUpdateDTO dto);
        ProfileRadarChartDTO GetProfileRadarChartDTO(int profileId);
        ProfileViewDTO GetProfileViewDTO(int profileId, Expression<Func<Profile, bool>> condition);
        void ModifyTokens(ProfileRoles profileRole, ProfileModifyTokensDTO dto);
    }

}