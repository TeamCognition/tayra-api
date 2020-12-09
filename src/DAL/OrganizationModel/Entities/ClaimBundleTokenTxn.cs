using System;

namespace Tayra.Models.Organizations
{
    public class ClaimBundleTokenTxn
    {
        public Guid ClaimBundleId { get; set; }
        public virtual ClaimBundle ClaimBundle { get; set; }

        public Guid TokenTransactionId { get; set; }
        public virtual TokenTransaction TokenTransaction { get; set; }
    }
}
