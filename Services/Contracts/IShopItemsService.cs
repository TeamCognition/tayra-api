using Firdaws.Core;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface IShopItemsService
    {
        ShopItemViewDTO GetShopItemViewDTO(int itemId);
        GridData<ShopItemViewGridDTO> GetShopItemViewGridDTO(ProfileRoles profileRole, ShopItemViewGridParams gridParams);
        void PurchaseShopItem(int profileId, ShopItemPurchaseDTO dto);
        ShopItem CreateShopItem(ShopItemCreateDTO dto);
        ShopItem UpdateShopItem(ShopItemUpdateDTO dto);
        void EnableShopItem(int itemId);
        void DisableShopItem(int itemId);
        void RemoveShopItem(int itemId);
    }
}
