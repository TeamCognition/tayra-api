using System.ComponentModel;

namespace Tayra.Common
{
    public enum ShopPurchaseStatuses
    {
        [Description("Pending Approval")]
        PendingApproval = 1,

        [Description("Processing")]
        Processing = 2,

        [Description("Fulfilled")]
        Fulfilled = 3,

        [Description("Refunded")]
        Refunded = 4
    }
}
