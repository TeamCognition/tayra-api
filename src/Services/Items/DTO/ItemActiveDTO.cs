using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class ItemActiveDTO
    {
        public Guid InventoryItemId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public ItemTypes Type { get; set; }
        public ItemRarities Rarity { get; set; }
    }
}
