using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public static class ClaimBundleRules
    {
        public static bool CanClaimReward(ClaimBundle claimBundle, int profileId)
        {
            return claimBundle.ProfileId == profileId;
        }
    }
}
