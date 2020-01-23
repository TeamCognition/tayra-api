using System;
using System.Collections.Generic;
using Firdaws.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public class ShopPurchasesGridParams : GridParams
    {
        public List<ItemTypes> ItemTypesQuery { get; set; }
        public List<ShopPurchaseStatuses> PurchaseStatusesQuery { get; set; }

        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
    }
}
