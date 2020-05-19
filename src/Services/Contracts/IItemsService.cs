using Firdaws.Core;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface IItemsService
    {
        ItemViewDTO GetItemViewDTO(ProfileRoles role, int itemId);
        GridData<ItemGridDTO> GetGridData(ProfileRoles role, ItemGridParams gridParams);
        Item CreateItem(ItemCreateDTO dto);
        Item UpdateItem(ItemUpdateDTO dto);
        void DeleteItem(int itemId);
    }
}
