using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public class ClaimBundleClaimRewardsDTO
    {
        public List<InventoryItem> ClaimedItems { get; set; }
        public List<Token> ClaimedTokens { get; set; }

        public class Token
        {
            public TokenType Type { get; set; }
            public double Value { get; set; }
        }

        public class InventoryItem
        {
            public string Name { get; set; }
            public string Image { get; set; }
        }
    }
}
