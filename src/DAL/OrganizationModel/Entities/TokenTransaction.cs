using System;
using System.ComponentModel.DataAnnotations;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class TokenTransaction : EntityGuidId, IClaimableEntity, ITimeStampedEntity
    {
        public double Value { get; set; }

        public string TxnHash { get; set; }

        [MaxLength(200)]
        public string Comment { get; set; }
        public TransactionReason Reason { get; set; }

        public Guid ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public TokenType TokenType { get; set; }
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
