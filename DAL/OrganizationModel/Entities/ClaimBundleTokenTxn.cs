namespace Tayra.Models.Organizations
{
    public class ClaimBundleTokenTxn
    {
        public int ClaimBundleId { get; set; }
        public virtual ClaimBundle ClaimBundle { get; set; }

        public int TokenTransactionId { get; set; }
        public virtual TokenTransaction TokenTransaction { get; set; }
    }
}
