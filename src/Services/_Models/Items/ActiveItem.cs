using System;
using Tayra.Common;

namespace Tayra.Services.Models.Items
{
    public class ActiveItem
    {
        public Guid InventoryItemId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public ItemTypes Type { get; set; }
        public ItemRarities Rarity { get; set; }
    }
}