using System;
using System.ComponentModel.DataAnnotations;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class ItemGift : ITimeStampedEntity
    {
        public int Id { get; set; }

        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        public int SenderId { get; set; }
        public virtual Profile Sender { get; set; }

        public int ReceiverId { get; set; }
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
