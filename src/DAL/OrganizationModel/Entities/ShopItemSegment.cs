using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class ShopItemSegment : ITimeStampedEntity
    {
        //Composite Key
        public Guid ShopItemId { get; set; }
        public virtual ShopItem ShopItem { get; set; }

        public Guid SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        public float? DiscountPrice { get; set; }
        public DateTime? DiscountEndsAt { get; set; }

        /// <summary>
        /// Date when item was hidden from a segment inventory shop.
        /// Only from global items
        /// </summary>
        public DateTime? HiddenAt { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
