using System;
using System.Linq;
using Firdaws.Core;
using Firdaws.DAL;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class ClaimBundlesService : BaseService<OrganizationDbContext>, IClaimBundlesService
    {
        #region Constructor

        public ClaimBundlesService(OrganizationDbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        IQueryable<ClaimBundle> ClaimBundleCommonScope() =>
            DbContext.ClaimBundles
                .Include(x => x.Items)
                    .ThenInclude(x => x.ProfileInventoryItem)
                    .ThenInclude(x => x.Item)
                .Include(x => x.TokenTxns)
                    .ThenInclude(x => x.TokenTransaction)
                    .ThenInclude(x => x.Token)
                .Where(x => x.RewardClaimedAt == null);

        #region Public Methods

        public GridData<ClaimBundleViewGridDTO> GetClaimBundlesGrid(int profileId, ClaimBundleViewGridParams gridParams)
        {
            var query = from c in DbContext.ClaimBundles
                        where c.ProfileId == profileId
                        where c.Type == gridParams.Type
                        where c.RewardClaimedAt == null
                        select new ClaimBundleViewGridDTO
                        {
                            Id = c.Id,
                            Type = c.Type,
                            Created = c.Created
                        };

            GridData<ClaimBundleViewGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public ClaimBundle CreateClaimBundle(int profileId, ClaimBundleTypes type)
        {
            var claimBundle = new ClaimBundle
            {
                ProfileId = profileId,
                Type = type
            };

            DbContext.Add(claimBundle);
            return claimBundle;
        }

        public ClaimBundleClaimRewardsDTO ClaimReward(int profileId, int claimBundleId)
        {
            var claimBundle = ClaimBundleCommonScope()
                .FirstOrDefault(x => x.Id == claimBundleId);

            claimBundle.EnsureNotNull(claimBundleId);

            if (!ClaimBundleRules.CanClaimReward(claimBundle, profileId))
            {
                throw new ApplicationException($"User cant claim this reward");
            }

            Claim(claimBundle, DateTime.UtcNow);

            return MapClaimRewardsDTO(claimBundle);
        }

        public ClaimBundleClaimRewardsDTO ClaimRewards(int profileId, ClaimBundleTypes type)
        { //TODO: merge claimRewards and this method
            var claimBundles = ClaimBundleCommonScope()
                .Where(x => x.ProfileId == profileId)
                .Where(x => x.Type == type)
                .ToArray();

            foreach (var cb in claimBundles)
            {
                if (!ClaimBundleRules.CanClaimReward(cb, profileId))
                {
                    throw new ApplicationException($"User cant claim this reward");
                }

                Claim(cb, DateTime.UtcNow);
            }

            return MapClaimRewardsDTO(claimBundles);
        }

        #endregion

        #region Private Methods

        private static void Claim(ClaimBundle claimBundle, DateTime claimedAt)
        {
            claimBundle.RewardClaimedAt = claimedAt;
            foreach (var cbItem in claimBundle.Items)
            {
                cbItem.ProfileInventoryItem.ClaimedAt = claimedAt;
            }

            foreach (var cbTTxn in claimBundle.TokenTxns)
            {
                cbTTxn.TokenTransaction.ClaimedAt = claimedAt;
            }
        }

        private static ClaimBundleClaimRewardsDTO MapClaimRewardsDTO(params ClaimBundle[] claimBundles)
        {
            return new ClaimBundleClaimRewardsDTO
            {
                ClaimedItems = claimBundles.SelectMany(x => x.Items).Select(x => new ClaimBundleClaimRewardsDTO.InventoryItem
                {
                    Image = x.ProfileInventoryItem.Item.Image,
                    Name = x.ProfileInventoryItem.Item.Name
                }).ToList(),

                ClaimedTokens = claimBundles
                    .SelectMany(x => x.TokenTxns)
                    .GroupBy(x => x.TokenTransaction.Token.Type)
                    .Select(x => new ClaimBundleClaimRewardsDTO.Token
                {
                    Type = x.Key,
                    Value = x.Sum(s => s.TokenTransaction.Value)
                }).ToList()
            };
        }
            

        #endregion
    }
}
