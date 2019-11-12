﻿using Tayra.Common;

namespace Tayra.Services
{
    public class ShopItemCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public bool IsActivable { get; set; }
        public bool IsDisenchantable { get; set; }
        public bool IsGiftable { get; set; }

        public ItemTypes Type { get; set; }
        public ItemRarities Rarity { get; set; }

        public int? Quantity { get; set; }
        public float Price { get; set; }
    }
}
