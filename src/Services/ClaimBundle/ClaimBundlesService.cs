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

        public ClaimBundleClaimRewardsDTO ShowAndClaimRewards(int profileId, int claimBundleId, bool claimRewards)
        {
            var claimBundle = ClaimBundleCommonScope()
                .FirstOrDefault(x => x.Id == claimBundleId);

            claimBundle.EnsureNotNull(claimBundleId);

            if (claimRewards)
            {
                Claim(profileId, DateTime.UtcNow, claimBundle);
            }

            return MapClaimRewardsDTO(claimBundle);
        }

        public ClaimBundleClaimRewardsDTO ShowAndClaimRewards(int profileId, ClaimBundleTypes type, bool claimRewards)
        { //TODO: merge claimRewards and this method
            var claimBundles = ClaimBundleCommonScope()
                .Where(x => x.ProfileId == profileId)
                .Where(x => x.Type == type)
                .ToArray();

            if (claimRewards)
            {
                Claim(profileId, DateTime.UtcNow, claimBundles);
            }

            return MapClaimRewardsDTO(claimBundles);
        }

        #endregion

        #region Private Methods

        private static void Claim(int profileId, DateTime claimedAt, params ClaimBundle[] claimBundles)
        {
            foreach (var cBundle in claimBundles)
            {
                if (!ClaimBundleRules.CanClaimReward(cBundle, profileId))
                {
                    throw new FirdawsSecurityException($"ProfileId {profileId} can't claim bundle with id: {cBundle.Id}");
                }

                cBundle.RewardClaimedAt = claimedAt;
                foreach (var cbItem in cBundle.Items)
                {
                    cbItem.ProfileInventoryItem.ClaimedAt = claimedAt;
                }

                foreach (var cbTTxn in cBundle.TokenTxns)
                {
                    cbTTxn.TokenTransaction.ClaimedAt = claimedAt;
                }
            }
        }

        private static ClaimBundleClaimRewardsDTO MapClaimRewardsDTO(params ClaimBundle[] claimBundles)
        {
            return new ClaimBundleClaimRewardsDTO
            {
                ClaimedItems = claimBundles.SelectMany(x => x.Items).Select(x => new ClaimBundleClaimRewardsDTO.InventoryItem
                {
                    ItemId = x.ProfileInventoryItem.ItemId,
                    Image = x.ProfileInventoryItem.Item.Image,
                    Name = x.ProfileInventoryItem.Item.Name,
                    Type = x.ProfileInventoryItem.Item.Type,
                    Rarity = x.ProfileInventoryItem.Item.Rarity
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
