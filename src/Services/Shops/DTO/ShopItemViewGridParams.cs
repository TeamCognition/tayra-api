using System;
using System.Collections.Generic;
using Firdaws.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public class ShopItemViewGridParams : GridParams
    {
        public string ItemNameQuery { get; set; }

        public int? ItemPriceFrom { get; set; }
        public int? ItemPriceTo { get; set; }

        public DateTime? ItemAddedFrom { get; set; }
        public DateTime? ItemAddedTo { get; set; }

        public List<ItemTypes> ItemTypesQuery { get; set; }
    }
}
