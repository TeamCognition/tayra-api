using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class ShopItemProject : ITimeStampedEntity
    {
        //Composite Key
        public int ShopItemId { get; set; }
        public virtual ShopItem ShopItem { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public float? DiscountPrice { get; set; }
        public DateTime? DiscountEndsAt { get; set; }

        /// <summary>
        /// Date when item was hidden from a project inventory shop.
        /// Only from global items
        /// </summary>
        public DateTime? HiddenAt { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
