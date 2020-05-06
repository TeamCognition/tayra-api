using Firdaws.Core;

namespace Tayra.Services
{
    public interface IPraiseWallService
    {
        void PraiseMember(int profileId, PraiseWallPraiseDTO dto);
        GridData<PraiseGridDTO> SearchPraises(PraiseSearchGridParams gridParams);
    }
}
