using System;
using System.ComponentModel.DataAnnotations;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class ItemGift : EntityGuidId, ITimeStampedEntity
    {
        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }

        public Guid SenderId { get; set; }
        public virtual Profile Sender { get; set; }

        public Guid ReceiverId { get; set; }
        public virtual Profile Receiver { get; set; }

        [MaxLength(250)]
        public string Message { get; set; }

        public int DateId { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
