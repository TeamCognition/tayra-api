using Cog.Core;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface IClaimBundlesService
    {
        GridData<ClaimBundleViewGridDTO> GetClaimBundlesGrid(int profileId, ClaimBundleViewGridParams gridParams);
        ClaimBundle CreateClaimBundle(int profileId, ClaimBundleTypes type);

        ClaimBundleClaimRewardsDTO ShowAndClaimRewards(int profileId, int claimBundleId, bool claimRewards);
        ClaimBundleClaimRewardsDTO ShowAndClaimRewards(int profileId, ClaimBundleTypes type, bool claimRewards);
    }
}
