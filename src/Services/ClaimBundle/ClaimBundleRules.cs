using System;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public static class ClaimBundleRules
    {
        public static bool CanClaimReward(ClaimBundle claimBundle, Guid profileId)
        {
            return claimBundle.ProfileId == profileId;
        }
    }
}
