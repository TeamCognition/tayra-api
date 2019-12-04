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
        Profile GetByNickname(string nickname);
        Profile GetByEmail(string email);
        Profile GetByExternalId(string externalId, IntegrationType integrationType);
        int OneUpProfile(int profileId, ProfileOneUpDTO dto);
        GridData<ProfileGridDTO> GetGridData(int profileId, ProfileGridParams gridParams);
        GridData<ProfileSummaryGridDTO> GetGridDataWithSummary(int profileId, ProfileSummaryGridParams gridParams);
        GridData<ProfileCompletedChallengesGridDTO> GetCompletedChallengesGridDTO(ProfileCompletedChallengesGridParams gridParams);        
        ProfileRadarChartDTO GetProfileRadarChartDTO(int profileId);
        ProfileViewDTO GetProfileViewDTO(Expression<Func<Profile, bool>> condition);
        void ModifyTokens(ProfileRoles profileRole, ProfileModifyTokensDTO dto);
    }

}