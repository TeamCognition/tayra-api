﻿using System;
using Newtonsoft.Json;
using Tayra.Common;

namespace Tayra.Services
{
    public class ShopPurchasesGridDTO
    {
        public int ShopPurchaseId { get; set; }
        public string BuyerUsername { get; set; }
        public float Price { get; set; }
        public ShopPurchaseStatuses Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        public ItemDTO Item { get; set; }

        [JsonIgnore]
        public ItemTypes ItemType { get; set; }

        public class ItemDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Image { get; set; }
            public ItemRarities Rarity { get; set; }
            public ItemTypes Type { get; set; }
        }
    }
}