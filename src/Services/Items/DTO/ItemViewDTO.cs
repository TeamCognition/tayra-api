using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class ItemViewDTO
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public float Price { get; set; }

        public bool IsActivable { get; set; }
        public bool IsDisenchantable { get; set; }
        public bool IsGiftable { get; set; }

        public ItemTypes Type { get; set; }
        public ItemRarities Rarity { get; set; }

        public int? ShopQuantityRemaining { get; set; }
        public int? ChallengesQuantityRemaining { get; set; }
        public int? GiveawayQuantityRemaining { get; set; }

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        public bool PlaceInShop { get; set; }
        public bool IsDisabled { get; set; }
    }
}
