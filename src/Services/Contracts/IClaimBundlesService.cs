using System;
using Cog.Core;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface IClaimBundlesService
    {
        GridData<ClaimBundleViewGridDTO> GetClaimBundlesGrid(Guid profileId, ClaimBundleViewGridParams gridParams);
        ClaimBundle CreateClaimBundle(Guid profileId, ClaimBundleTypes type);

        ClaimBundleClaimRewardsDTO ShowAndClaimRewards(Guid profileId, Guid claimBundleId, bool claimRewards);
        ClaimBundleClaimRewardsDTO ShowAndClaimRewards(Guid profileId, ClaimBundleTypes type, bool claimRewards);
    }
}
