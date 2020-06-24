using Cog.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public interface IShopItemsService
    {
        ShopItemViewDTO GetShopItemViewDTO(int itemId);
        GridData<ShopItemViewGridDTO> GetShopItemViewGridDTO(ProfileRoles profileRole, ShopItemViewGridParams gridParams);
        void PurchaseShopItem(int profileId, ShopItemPurchaseDTO dto);
        void EnableShopItem(int itemId);
        void DisableShopItem(int itemId);
        void RemoveShopItem(int itemId);
    }
}
