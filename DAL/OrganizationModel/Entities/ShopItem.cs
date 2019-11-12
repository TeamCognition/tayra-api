using System;
using System.Collections.Generic;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    /// <summary>
    /// Shop Inventory Items
    /// </summary>
    public class ShopItem : IAuditedEntity
    {
        public int Id { get; set; }

        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        public int? Quantity { get; set; }
        
        public float Price { get; set; }
        public float? DiscountPrice { get; set; }
        public DateTime? DiscountEndsAt { get; set; }
        public DateTime? FeaturedUntil { get; set; }

        public DateTime? DisabledAt { get; set; }
        public DateTime? ArchivedAt { get; set; }

        public bool IsGlobal { get; set; }
        public virtual ICollection<ShopItemProject> Projects { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}