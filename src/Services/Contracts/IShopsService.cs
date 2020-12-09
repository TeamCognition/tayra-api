using System;
using Cog.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public interface IShopsService
    {
        ShopViewDTO GetShopViewDTO(Guid profileId, ProfileRoles role);
        GridData<ShopPurchasesGridDTO> GetShopPurchasesGridDTO(Guid profileId, ProfileRoles role, ShopPurchasesGridParams gridParams);
        void UpdateShopPurchaseStatus(Guid profileId, Guid shopPurchaseId, ShopPurchaseStatuses newStatus);
        void OpenShop();
        void CloseShop();
        ShopTokenAverageEarningsDTO GetTokenAverageEarnings();
    }
}
