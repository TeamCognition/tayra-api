using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class InventoryItemGridDTO
    {
        public Guid InventoryItemId { get; set; }
        public Guid ItemId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public bool IsActive { get; set; }
        public ItemTypes Type { get; set; }
        public ItemRarities Rarity { get; set; }

        public bool IsActivable { get; set; }
        public bool IsDisenchantable { get; set; }
        public bool IsGiftable { get; set; }

        public InventoryAcquireMethods AcquireMethod { get; set; }
        public float Price { get; set; }

        public DateTime Created { get; set; }
    }
}
