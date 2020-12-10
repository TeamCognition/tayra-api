using System;
using System.Linq;
using Cog.Core;
using Cog.DAL;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class ShopsService : BaseService<OrganizationDbContext>, IShopsService
    {
        #region Constructor

        protected ITokensService TokensService { get; set; }
        protected ILogsService LogsService { get; set; }

        public ShopsService(ITokensService tokensService, ILogsService logsService, OrganizationDbContext dbContext) : base(dbContext)
        {
            TokensService = tokensService;
            LogsService = logsService;
        }

        #endregion

        #region Public Methods

        public ShopViewDTO GetShopViewDTO(Guid profileId, ProfileRoles role)
        {
            var shopDto = (from s in DbContext.Shops
                               //where s.Id == shopId
                           select new ShopViewDTO
                           {
                               Name = s.Name,
                               IsClosed = s.ClosedAt.HasValue,
                               Created = s.Created
                           }).FirstOrDefault();

            shopDto.EnsureNotNull();

            shopDto.TotalRequests = DbContext.ShopPurchases.Count(x =>
                x.Status == ShopPurchaseStatuses.PendingApproval || x.Status == ShopPurchaseStatuses.PendingApproval);
            
            //some of this code can be shared when reports become more generic
            if (role == ProfileRoles.Member)
            {
                // var stats = (from r in DbContext.ProfileReportsDaily
                //              where r.ProfileId == profileId
                //              group r by 1 into total
                //              let last30 = total.Where(x => x.DateId >= DateHelper2.ToDateId(DateTime.UtcNow.AddDays(-30)))
                //              select new[]
                //              {
                //                 new ShopViewDTO.ShopStatisticDTO
                //                 {
                //                     Last30 = last30.Sum(x => x.ItemsBoughtChange),
                //                     Total = total.Sum(x => x.ItemsBoughtChange)
                //                 },
                //                 new ShopViewDTO.ShopStatisticDTO
                //                 {
                //                     Last30 = last30.Sum(x => x.CompanyTokensSpentChange),
                //                     Total = total.Sum(x => x.CompanyTokensSpentChange)
                //                 }
                //             }).FirstOrDefault();

                // if (stats != null)
                // {
                    shopDto.ItemStats =  new ShopViewDTO.ShopStatisticDTO
                    {
                        Last30 = -1,
                        Total = -1
                    };
                    shopDto.TokenStats = new ShopViewDTO.ShopStatisticDTO
                    {
                        Last30 = -1,
                        Total = -1
                    };
                // }
            }
            else
            {
                IQueryable<SegmentReportDaily> rQuery = DbContext.SegmentReportsDaily;

                if (role == ProfileRoles.Manager)
                {
                    var managersSegmentsIds = DbContext.ProfileAssignments.Where(x => x.ProfileId == profileId && x.TeamId == null).Select(x => x.SegmentId).ToArray();
                    rQuery = rQuery.Where(x => managersSegmentsIds.Contains(x.SegmentId));
                }

                // var stats = (from r in rQuery
                //              group r by 1 into total
                //              let last30 = total.Where(x => x.DateId >= 100)
                //              select new[]
                //              {
                //                 new ShopViewDTO.ShopStatisticDTO
                //                 {
                //                     Last30 = last30.Sum(x => x.ItemsBoughtChange),
                //                     Total = total.Sum(x => x.ItemsBoughtChange)
                //                 },
                //                 new ShopViewDTO.ShopStatisticDTO
                //                 {
                //                     Last30 = last30.Sum(x => x.CompanyTokensSpentChange),
                //                     Total = total.Sum(x => x.CompanyTokensSpentChange)
                //                 }
                //             }).FirstOrDefault();
                //
                // if (stats != null)
                // {
                //     shopDto.ItemStats = stats[0];
                //     shopDto.TokenStats = stats[1];
                // }
                shopDto.ItemStats =  new ShopViewDTO.ShopStatisticDTO
                {
                    Last30 = -1,
                    Total = -1
                };
                shopDto.TokenStats = new ShopViewDTO.ShopStatisticDTO
                {
                    Last30 = -1,
                    Total = -1
                };
            }

            return shopDto;
        }

        public GridData<ShopPurchasesGridDTO> GetShopPurchasesGridDTO(Guid profileId, ProfileRoles role, ShopPurchasesGridParams gridParams)
        {
            var scope = role == ProfileRoles.Member
                ? DbContext.ShopPurchases.Where(x => x.ProfileId == profileId)
                : DbContext.ShopPurchases;

            if (!string.IsNullOrEmpty(gridParams.ItemNameQuery))
            {
                scope = scope.Where(x => x.Item.Name.Contains(gridParams.ItemNameQuery));
            }

            var query = from sp in scope
                        select new ShopPurchasesGridDTO
                        {
                            ShopPurchaseId = sp.Id,
                            FirstName = sp.Profile.FirstName,
                            LastName = sp.Profile.LastName,
                            Avatar = sp.Profile.Avatar,
                            BuyerUsername = sp.Profile.Username,
                            Price = sp.Price,
                            Status = sp.Status,
                            Created = sp.Created,
                            LastModified = sp.LastModified ?? sp.Created,
                            ItemType = sp.ItemType,
                            Item = new ShopPurchasesGridDTO.ItemDTO
                            {
                                Id = sp.ItemId,
                                Name = sp.Item.Name,
                                Image = sp.Item.Image,
                                Rarity = sp.Item.Rarity,
                                Type = sp.Item.Type
                            }
                        };

            GridData<ShopPurchasesGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public void UpdateShopPurchaseStatus(Guid profileId, Guid shopPurchaseId, ShopPurchaseStatuses newStatus)
        {
            var shopPurchase = DbContext.ShopPurchases.Include(x => x.Item).FirstOrDefault(x => x.Id == shopPurchaseId);

            shopPurchase.EnsureNotNull(shopPurchaseId);

            if (!ShopRules.CanUpdateShopPurchaseStatus(shopPurchase.Status, newStatus))
            {
                throw new ApplicationException("Can't update to this status");
            }

            shopPurchase.Status = newStatus;
            shopPurchase.LastModifiedDateId = DateHelper2.ToDateId(DateTime.UtcNow);

            if (newStatus == ShopPurchaseStatuses.Fulfilled)
            {
                DbContext.Add(new ProfileInventoryItem
                {
                    ItemId = shopPurchase.ItemId,
                    ProfileId = shopPurchase.ProfileId,
                    AcquireMethod = InventoryAcquireMethods.ShopPurchase,
                    IsActive = false,
                    ItemType = shopPurchase.Item.Type
                });
            }
            else if (newStatus == ShopPurchaseStatuses.Refunded)
            {
                TokensService.CreateTransaction(TokenType.CompanyToken, shopPurchase.ProfileId, shopPurchase.Price, TransactionReason.ShopItemRefund, null);
            }
        }

        public void OpenShop()
        {
            var shop = DbContext.Shops.FirstOrDefault();

            shop.EnsureNotNull();

            shop.ClosedAt = null;
        }

        public void CloseShop()
        {
            var shop = DbContext.Shops.FirstOrDefault();

            shop.EnsureNotNull();

            shop.ClosedAt = DateTime.UtcNow;
        }

        public ShopTokenAverageEarningsDTO GetTokenAverageEarnings()
        {
            return new ShopTokenAverageEarningsDTO
            {
                AverageEarnings = DbContext.SegmentReportsWeekly.Select(x => x.CompanyTokensEarnedChange)
                    .DefaultIfEmpty(0)
                    .Average()
            };
        }

        #endregion
    }
}
