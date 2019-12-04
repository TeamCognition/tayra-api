namespace Tayra.Models.Organizations
{
    public class ClaimBundleItem
    {
        public int ClaimBundleId { get; set; }
        public virtual ClaimBundle ClaimBundle { get; set; }

        public int ProfileInventoryItemId { get; set; }
        public virtual ProfileInventoryItem ProfileInventoryItem { get; set; }
    }
}
