using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class ItemReservation : ITimeStampedEntity
    {
        public int Id { get; set; }

        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        public int QuantityChange { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
