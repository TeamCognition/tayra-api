using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ShopPurchase : Entity<Guid>, ITimeStampedEntity
    {
        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }

        public Guid ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public Guid? SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        public ShopPurchaseStatuses Status { get; set; }
        public ItemTypes ItemType { get; set; } //for read-only purposes

        public bool IsFeatured { get; set; }
        public bool IsDiscounted { get; set; }

        public int? GiftFor { get; set; }

        public int LastModifiedDateId { get; set; }

        /// <summary>
        /// Price Paid
        /// </summary>
        public float Price { get; set; }
        public float? PriceDiscountedFor { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
