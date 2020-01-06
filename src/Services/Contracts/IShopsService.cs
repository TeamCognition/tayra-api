using Firdaws.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public interface IShopsService
    {
        ShopViewDTO GetShopViewDTO(int shopId);
        GridData<ShopPurchasesGridDTO> GetShopPurchasesGridDTO(int profileId, ProfileRoles role, ShopPurchasesGridParams gridParams);
        void UpdateShopPurchaseStatus(int profileId, int shopPurchaseId, ShopPurchaseStatuses newStatus);
        void OpenShop(int shopId);
        void CloseShop(int shopId);
    }
}
