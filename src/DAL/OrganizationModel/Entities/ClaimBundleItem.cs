using System;

namespace Tayra.Models.Organizations
{
    public class ClaimBundleItem
    {
        public Guid ClaimBundleId { get; set; }
        public virtual ClaimBundle ClaimBundle { get; set; }

        public Guid ProfileInventoryItemId { get; set; }
        public virtual ProfileInventoryItem ProfileInventoryItem { get; set; }
    }
}
