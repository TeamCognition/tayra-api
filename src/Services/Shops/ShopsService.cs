using System;
using System.Linq;
using Firdaws.Core;
using Firdaws.DAL;
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

        public ShopViewDTO GetShopViewDTO(int profileId,ProfileRoles role)
        {
            
            var shopDto = (from s in DbContext.Shops
                               //where s.Id == shopId
                           select new ShopViewDTO
                           {
                               Name = s.Name,
                               IsClosed = s.ClosedAt.HasValue,
                               Created = s.Created,
                               
                           }).FirstOrDefault();

            string roleName = role.ToString();

            if (roleName == "Admin")
            {
                var iblm = (from srd in DbContext.SegmentReportsDaily
                            orderby srd.DateId descending
                            select new { srd.ItemsBoughtChange }).Take(30).ToArray();

                int itemsBoughtLastMonth = iblm.Sum(x => x.ItemsBoughtChange);
                
                var totalItemsBought = (from srd in DbContext.SegmentReportsDaily
                                      orderby srd.DateId descending
                                      group srd by srd.SegmentId into r
                                      select new { segmentsTotal = r.Sum(x => x.ItemsBoughtTotal) }).Sum(x => x.segmentsTotal);

                var tslm = (from srd in DbContext.SegmentReportsDaily
                            orderby srd.DateId descending
                            select new { srd.CompanyTokensSpentChange }).Take(30).ToArray();

                float tokensSpentLastMonth = tslm.Sum(x => x.CompanyTokensSpentChange);

                var totalTokensSpent = (from srd in DbContext.SegmentReportsDaily
                                        orderby srd.DateId descending
                                        group srd by srd.SegmentId into r
                                        select new { segmentTotal = r.Sum(x => x.CompanyTokensSpentChange) }).Sum(x => x.segmentTotal);

                shopDto.shopStatistics = new ShopViewDTO.ShopStatisticDTO[]
                {
                    new ShopViewDTO.ShopStatisticDTO
                    {
                        key="items",
                        lastMonth = itemsBoughtLastMonth,
                        total = totalItemsBought
                    },
                    new ShopViewDTO.ShopStatisticDTO
                    {
                        key="tokens",
                        lastMonth = tokensSpentLastMonth,
                        total = totalTokensSpent
                    }
                };
            } 
            else if (roleName == "Manager")
            {
                var managersSegmentsIds = DbContext.ProfileAssignments.Where(x => x.ProfileId == profileId && x.TeamId == null).Select(x => x.SegmentId).ToArray();

                var iblm = (from srd in DbContext.SegmentReportsDaily
                            where managersSegmentsIds.Contains(srd.SegmentId)
                            orderby srd.DateId descending
                            select new { srd.ItemsBoughtChange }).Take(30).ToArray();

                int itemsBoughtLastMonth = iblm.Sum(x => x.ItemsBoughtChange);

                var totalItemsBought = (from srd in DbContext.SegmentReportsDaily
                                        where managersSegmentsIds.Contains(srd.SegmentId)
                                        orderby srd.DateId descending
                                        group srd by srd.SegmentId into r
                                        select new { segmentsTotal = r.Sum(x => x.ItemsBoughtTotal) }).Sum(x => x.segmentsTotal);

                var tslm = (from srd in DbContext.SegmentReportsDaily
                            where managersSegmentsIds.Contains(srd.SegmentId)
                            orderby srd.DateId descending
                            select new { srd.CompanyTokensSpentChange }).Take(30).ToArray();

                float tokensSpentLastMonth = tslm.Sum(x => x.CompanyTokensSpentChange);

                var totalTokensSpent = (from srd in DbContext.SegmentReportsDaily
                                        where managersSegmentsIds.Contains(srd.SegmentId)
                                        orderby srd.DateId descending
                                        group srd by srd.SegmentId into r
                                        select new { segmentTotal = r.Sum(x => x.CompanyTokensSpentChange) }).Sum(x => x.segmentTotal);

                shopDto.shopStatistics = new ShopViewDTO.ShopStatisticDTO[]
                {
                    new ShopViewDTO.ShopStatisticDTO
                    {
                        key="items",
                        lastMonth = itemsBoughtLastMonth,
                        total = totalItemsBought
                    },
                    new ShopViewDTO.ShopStatisticDTO
                    {
                        key="tokens",
                        lastMonth = tokensSpentLastMonth,
                        total = totalTokensSpent
                    }
                };
            } 
            else if (roleName == "Member")
            {
                var userData = DbContext.ProfileReportsDaily.OrderByDescending(x => x.DateId).Where(x => x.ProfileId == profileId).Select( x => new { x.ItemsBoughtTotal, x.CompanyTokensSpentTotal}).ToList();

                var iblm = (from prd in DbContext.ProfileReportsDaily
                            orderby prd.DateId descending
                            where prd.ProfileId == profileId
                            select new { prd.ItemsBoughtChange }).ToArray();

                int itemsBoughtLastMonth = iblm.Sum(x => x.ItemsBoughtChange);

                var totalItemsBought = userData.Select(x => x.ItemsBoughtTotal).FirstOrDefault();
                
                var tslm = (from prd in DbContext.ProfileReportsDaily
                            orderby prd.DateId descending
                            where prd.ProfileId == profileId
                            select new { prd.CompanyTokensSpentChange }).Take(30).ToArray();

                float tokensSpentLastMonth = tslm.Sum(x => x.CompanyTokensSpentChange);

                float totalTokensSpent = userData.Select(x => x.CompanyTokensSpentTotal).FirstOrDefault();

                shopDto.shopStatistics = new ShopViewDTO.ShopStatisticDTO[]
                {
                    new ShopViewDTO.ShopStatisticDTO
                    {
                        key="items",
                        lastMonth = totalItemsBought,
                        total = itemsBoughtLastMonth
                    },
                    new ShopViewDTO.ShopStatisticDTO
                    {
                        key="tokens",
                        lastMonth = tokensSpentLastMonth,
                        total = totalTokensSpent
                    }
                };   
            }

            shopDto.EnsureNotNull();
            
            return shopDto;
        }

        public GridData<ShopPurchasesGridDTO> GetShopPurchasesGridDTO(int profileId, ProfileRoles role, ShopPurchasesGridParams gridParams)
        {
            var scope = role == ProfileRoles.Member
                ? DbContext.ShopPurchases.Where(x => x.ProfileId == profileId)
                : DbContext.ShopPurchases;

            if(!string.IsNullOrEmpty(gridParams.ItemNameQuery))
            {
                scope = scope.Where(x => x.Item.Name.Contains(gridParams.ItemNameQuery));
            }

            var query = from sp in scope
                        select new ShopPurchasesGridDTO
                        {
                            ShopPurchaseId = sp.Id,
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

        public void UpdateShopPurchaseStatus(int profileId, int shopPurchaseId, ShopPurchaseStatuses newStatus)
        {
            var shopPurchase = DbContext.ShopPurchases.Include(x => x.Item).FirstOrDefault(x => x.Id == shopPurchaseId);

            shopPurchase.EnsureNotNull(shopPurchaseId);

            if (ShopRules.CanUpdateShopPurchaseStatus(shopPurchase.Status, newStatus))
            {
                throw new ApplicationException("Can't update to this status");
            }

            shopPurchase.Status = newStatus;

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

        public void OpenShop(int shopId)
        {
            var shop = DbContext.Shops.FirstOrDefault(x => x.Id == shopId);

            shop.EnsureNotNull(shopId);

            shop.ClosedAt = null;
        }

        public void CloseShop(int shopId)
        {
            var shop = DbContext.Shops.FirstOrDefault(x => x.Id == shopId);

            shop.EnsureNotNull(shopId);

            shop.ClosedAt = DateTime.UtcNow;
        }

        #endregion
    }
}
