using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class InventoryItemViewDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public float Price { get; set; }

        public bool IsActivable { get; set; }
        public bool IsDisenchantable { get; set; }
        public bool IsGiftable { get; set; }

        public bool IsActive { get; set; }
        public ItemTypes Type { get; set; }
        public ItemRarities Rarity { get; set; }

        public InventoryAcquireMethods AcquireMethod { get; set; }
        public string AcquireDetail { get; set; }

        public DateTime Created { get; set; }
    }
}
