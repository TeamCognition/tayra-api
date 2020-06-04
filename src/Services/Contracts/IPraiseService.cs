using Cog.Core;

namespace Tayra.Services
{
    public interface IPraiseService
    {
        void PraiseProfile(int profileId, PraiseProfileDTO dto);
        GridData<PraiseSearchGridDTO> SearchPraises(PraiseSearchGridParams gridParams);
    }
}
