using System;
using System.ComponentModel.DataAnnotations;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class TokenTransaction : IClaimableEntity, ITimeStampedEntity
    {
        [Key]
        public int Id { get; set; }

        public double Value { get; set; }
        public double FinalBalance { get; set; }

        public string TxnHash { get; set; }

        [MaxLength(200)]
        public string Comment { get; set; }
        public TransactionReason Reason { get; set; }

        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int TokenId { get; set; }
        public virtual Token Token { get; set; }

        public int DateId { get; set; }

        #region IClaimableEntity

        public bool ClaimRequired { get; set; }
        public DateTime? ClaimedAt { get; set; }

        #endregion

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
