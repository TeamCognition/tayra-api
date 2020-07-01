using System;
using Cog.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class ClaimBundlesController : BaseController
    {
        #region Constructor

        public ClaimBundlesController(IServiceProvider serviceProvider, OrganizationDbContext context) : base(serviceProvider)
        {
            OrganizationContext = context;
        }

        public OrganizationDbContext OrganizationContext { get; set; }

        #endregion

        #region Action Methods

        [HttpPost("search")]
        public ActionResult<GridData<ClaimBundleViewGridDTO>> GetSegmentQuests([FromBody] ClaimBundleViewGridParams gridParams)
        {
            return ClaimBundlesService.GetClaimBundlesGrid(CurrentUser.ProfileId, gridParams);
        }

        [HttpGet]
        public ActionResult<ClaimBundleClaimRewardsDTO> ShowClaimBundle([FromQuery] int? claimBundleId, [FromQuery] ClaimBundleTypes? claimBundleType)
        {
            if (!claimBundleId.HasValue && !claimBundleType.HasValue)
            {
                throw new ApplicationException("provide claimBundleId or claimBundleType");
            }

            ClaimBundleClaimRewardsDTO claimedRewards;
            if (claimBundleId.HasValue)
            {
                claimedRewards = ClaimBundlesService.ShowAndClaimRewards(CurrentUser.ProfileId, claimBundleId.Value, false);
            }
            else
            {
                claimedRewards = ClaimBundlesService.ShowAndClaimRewards(CurrentUser.ProfileId, claimBundleType.Value, false);
            }

            return Ok(claimedRewards);
        }

        [HttpPost("claim")]
        public ActionResult<ClaimBundleClaimRewardsDTO> ClaimClaimBundle([FromQuery] int? claimBundleId, [FromQuery] ClaimBundleTypes? claimBundleType)
        {
            if(!claimBundleId.HasValue && !claimBundleType.HasValue)
            {
                throw new ApplicationException("provide claimBundleId or claimBundleType");
            }

            ClaimBundleClaimRewardsDTO claimedRewards;
            if (claimBundleId.HasValue)
            {
                claimedRewards = ClaimBundlesService.ShowAndClaimRewards(CurrentUser.ProfileId, claimBundleId.Value, true);
            }
            else
            {
                claimedRewards = ClaimBundlesService.ShowAndClaimRewards(CurrentUser.ProfileId, claimBundleType.Value, true);
            }

            OrganizationContext.SaveChanges();

            return Ok(claimedRewards);
        }

        #endregion
    }
}
