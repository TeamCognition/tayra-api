using Tayra.Common;

namespace Tayra.Services
{
    public static class ShopRules
    {
        public static bool CanPurchaseItem(bool isShopClosed, double profileTokenBalance, float shopItemPrice, int? shopItemQuantity )
        {
            return !isShopClosed
                && (shopItemQuantity == null || shopItemQuantity > 0)
                && profileTokenBalance >= shopItemPrice;
        }

        public static bool CanUpdateShopPurchaseStatus(ShopPurchaseStatuses currentStatus, ShopPurchaseStatuses newStatus)
        {
            return newStatus > currentStatus && currentStatus != ShopPurchaseStatuses.Fulfilled;
        }
    }
}
