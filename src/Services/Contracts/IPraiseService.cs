using System;
using Cog.Core;

namespace Tayra.Services
{
    public interface IPraiseService
    {
        void PraiseProfile(Guid profileId, PraiseProfileDTO dto);
        GridData<PraiseSearchGridDTO> SearchPraises(PraiseGridParams gridParams);
        GridData<PraiseSearchProfilesDTO> SearchProfiles(PraiseProfileSearchGridParams gridParams);
    }
}
