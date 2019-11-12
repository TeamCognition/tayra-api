using System;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ShopPurchase : ITimeStampedEntity
    {
        public int Id { get; set; }

        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public ShopPurchaseStatuses Status { get; set; }
        public ItemTypes ItemType { get; set; } //for read-only purposes

        public bool IsFeatured { get; set; }
        public bool IsDiscounted { get; set; }

        public int? GiftFor { get; set; }

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
