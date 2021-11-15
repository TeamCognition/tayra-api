using System;
using Cog.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public interface IShopItemsService
    {
        ShopItemViewDTO GetShopItemViewDTO(Guid itemId);
        GridData<ShopItemViewGridDTO> GetShopItemViewGridDTO(ProfileRoles profileRole, ShopItemViewGridParams gridParams);
        void PurchaseShopItem(Guid profileId, ShopItemPurchaseDTO dto);
        void EnableShopItem(Guid itemId);
        void DisableShopItem(Guid itemId);
        void RemoveShopItem(Guid itemId);
    }
}
