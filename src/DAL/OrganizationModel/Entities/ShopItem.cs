using System;
using System.Collections.Generic;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    /// <summary>
    /// Shop Inventory Items
    /// </summary>
    public class ShopItem : Entity<Guid>, IAuditedEntity, IArchivableEntity
    {
        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }

        public float? DiscountPrice { get; set; }
        public DateTime? DiscountEndsAt { get; set; }
        public DateTime? FeaturedUntil { get; set; }

        public DateTime? DisabledAt { get; set; }

        public bool IsGlobal { get; set; }
        public virtual ICollection<ShopItemSegment> Segments { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}