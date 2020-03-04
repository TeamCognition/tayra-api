﻿using System;
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

        public ShopViewDTO GetShopViewDTO()
        {

            var totalItemsSold = (from srd in DbContext.SegmentReportsDaily
                          orderby srd.DateId descending
                          group srd by srd.SegmentId into r
                          select new { segmentTotal = r.Sum(x => x.ItemsBoughtTotal) }).Sum(x=>x.segmentTotal);

            var islm = (from srd in DbContext.SegmentReportsDaily
                                     orderby srd.DateId descending
                                     select new { srd.ItemsBoughtChange }).Take(30).ToArray();

            var itemsSoldLastMonth = islm.Sum(x => x.ItemsBoughtChange);

            var shopDto = (from s in DbContext.Shops
                               //where s.Id == shopId
                           select new ShopViewDTO
                           {
                               Name = s.Name,
                               IsClosed = s.ClosedAt.HasValue,
                               Created = s.Created,
                               shopStatistics = new ShopViewDTO.ShopStatisticDTO[] {
                                    new ShopViewDTO.ShopStatisticDTO {
                                        lastMonth = itemsSoldLastMonth,
                                        total = totalItemsSold
                                    } 
                               }
                           }).FirstOrDefault();

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
