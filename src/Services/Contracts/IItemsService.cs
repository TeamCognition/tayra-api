using Firdaws.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public interface IItemsService
    {
        ItemViewDTO GetItemViewDTO(ProfileRoles role, int itemId);
        GridData<ItemGridDTO> GetGridData(ProfileRoles role, ItemGridParams gridParams);
    }
}
