using System.Collections.Generic;
using Firdaws.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public class ShopPurchasesAdminGridParams : GridParams
    {
        public List<ItemTypes> ItemTypesQuery { get; set; }
        public List<ShopPurchaseStatuses> PurchaseStatusesQuery { get; set; }
    }
}
