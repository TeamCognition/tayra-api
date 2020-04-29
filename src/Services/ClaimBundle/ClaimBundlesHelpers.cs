using System.Collections.Generic;
using System.Linq;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public static class ClaimBundlesHelpers
    {
        #region Static methods

        public static ClaimBundle AddItems(this ClaimBundle claimBundle, params ProfileInventoryItem[] inventoryItems)
        {
            if (claimBundle.Items == null)
            {
                claimBundle.Items = new List<ClaimBundleItem>();
            }

            claimBundle.Items.AddRange(inventoryItems.Select(x => new ClaimBundleItem { ProfileInventoryItem = x }).ToList());
            return claimBundle;
        }

        public static ClaimBundle AddTokenTxns(this ClaimBundle claimBundle, params TokenTransaction[] txns)
        {
            if (claimBundle.TokenTxns == null)
            {
                claimBundle.TokenTxns = new List<ClaimBundleTokenTxn>();
            }

            claimBundle.TokenTxns.AddRange(txns.Select(x => new ClaimBundleTokenTxn { TokenTransaction = x }).ToList());
            return claimBundle;
        }

        #endregion
    }
}
