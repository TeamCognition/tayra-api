using System;
using System.Collections.Generic;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ClaimBundle : ITimeStampedEntity
    {
        public int Id { get; set; }

        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public ClaimBundleTypes Type { get; set; }

        public DateTime? RewardClaimedAt { get; set; }

        public virtual List<ClaimBundleItem> Items { get; set; }
        public virtual List<ClaimBundleTokenTxn> TokenTxns { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
