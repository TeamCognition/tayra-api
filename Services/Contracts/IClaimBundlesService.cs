using Firdaws.Core;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface IClaimBundlesService
    {
        GridData<ClaimBundleViewGridDTO> GetClaimBundlesGrid(int profileId, ClaimBundleViewGridParams gridParams);
        ClaimBundle CreateClaimBundle(int profileId, ClaimBundleTypes type);

        ClaimBundleClaimRewardsDTO ClaimReward(int profileId, int claimBundleId);
        ClaimBundleClaimRewardsDTO ClaimRewards(int profileId, ClaimBundleTypes type);
    }
}
