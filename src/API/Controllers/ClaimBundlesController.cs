using System;
using Firdaws.Core;
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
        public ActionResult<GridData<ClaimBundleViewGridDTO>> GetProjectChallenges([FromBody] ClaimBundleViewGridParams gridParams)
        {
            if (string.IsNullOrEmpty(gridParams.Sidx))
            {
                gridParams.Sidx = nameof(ClaimBundleViewGridDTO.Created);
                gridParams.Sord = "DESC";
            }

            return ClaimBundlesService.GetClaimBundlesGrid(CurrentUser.ProfileId, gridParams);
        }

        [HttpPost("claim")]
        public ActionResult<ClaimBundleClaimRewardsDTO> ClaimClaimBundle([FromQuery] int? claimBundleId, [FromQuery] ClaimBundleTypes? claimBundleType)
        {
            if(!claimBundleId.HasValue && !claimBundleType.HasValue)
            {
                throw new ApplicationException("provide claimBundleId or type");
            }

            ClaimBundleClaimRewardsDTO claimedRewards;
            if (claimBundleId.HasValue)
            {
                claimedRewards = ClaimBundlesService.ClaimReward(CurrentUser.ProfileId, claimBundleId.Value);
            }
            else
            {
                claimedRewards = ClaimBundlesService.ClaimRewards(CurrentUser.ProfileId, claimBundleType.Value);
            }

            OrganizationContext.SaveChanges();

            return Ok(claimedRewards);
        }

        #endregion
    }
}
