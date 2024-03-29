﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Cog.DAL;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class ShopItemsService : BaseService<OrganizationDbContext>, IShopItemsService
    {
        #region Constructor

        protected ILogsService LogsService { get; set; }
        protected ITokensService TokensService { get; set; }

        public ShopItemsService(ILogsService logsService, ITokensService tokensService, OrganizationDbContext dbContext) : base(dbContext)
        {
            LogsService = logsService;
            TokensService = tokensService;
        }

        #endregion

        #region Public Methods

        public ShopItemViewDTO GetShopItemViewDTO(Guid itemId)
        {
            var shopItemDto = (from si in DbContext.ShopItems
                               where si.ItemId == itemId
                               select new ShopItemViewDTO
                               {
                                   ItemId = si.ItemId,
                                   Created = si.Created,
                                   IsDisabled = si.DisabledAt.HasValue,
                                   Quantity = si.Item.ShopQuantityRemaining,
                                   Price = si.Item.Price,
                                   Name = si.Item.Name,
                                   Description = si.Item.Description,
                                   Image = si.Item.Image,
                                   Type = si.Item.Type,
                                   IsActivable = si.Item.IsActivable,
                                   IsDisenchantable = si.Item.IsDisenchantable,
                                   IsGiftable = si.Item.IsGiftable,
                                   Rarity = si.Item.Rarity,
                               }).FirstOrDefault();

            shopItemDto.EnsureNotNull(itemId);

            return shopItemDto;
        }

        public GridData<ShopItemViewGridDTO> GetShopItemViewGridDTO(ProfileRoles profileRole, ShopItemViewGridParams gridParams)
        {
            IQueryable<ShopItem> scope = DbContext.ShopItems;

            if (!string.IsNullOrEmpty(gridParams.ItemNameQuery))
            {
                scope = scope.Where(x => x.Item.Name.Contains(gridParams.ItemNameQuery));
            }

            var query = from si in scope
                        where profileRole != ProfileRoles.Member || si.DisabledAt == null
                        select new ShopItemViewGridDTO
                        {
                            ItemId = si.ItemId,
                            Created = si.Created,
                            IsDisabled = si.DisabledAt.HasValue,
                            Quantity = si.Item.ShopQuantityRemaining,
                            Price = si.Item.Price,
                            Name = si.Item.Name,
                            Description = si.Item.Description,
                            Image = si.Item.Image,
                            Type = si.Item.Type,
                            IsActivable = si.Item.IsActivable,
                            IsDisenchantable = si.Item.IsDisenchantable,
                            IsGiftable = si.Item.IsGiftable,
                            Rarity = si.Item.Rarity
                        };

            GridData<ShopItemViewGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public void PurchaseShopItem(Guid profileId, ShopItemPurchaseDTO dto)
        {
            var shop = DbContext.Shops.FirstOrDefault();
            var shopItem = DbContext.ShopItems.Include(x => x.Item /*for logs and price*/).FirstOrDefault(x => x.ItemId == dto.ItemId);
            var profileTokenBalance = DbContext.TokenTransactions.Where(x => x.ProfileId == profileId && x.TokenType == TokenType.CompanyToken).Sum(x => x.Value);
            var segmentId = DbContext.ProfileAssignments.Where(x => x.ProfileId == profileId).Select(x => (Guid?)x.SegmentId).FirstOrDefault();
            
            shopItem.EnsureNotNull(shop.Id, dto.ItemId);

            if (!dto.DemoDate.HasValue && !ShopRules.CanPurchaseItem(shop.ClosedAt.HasValue, profileTokenBalance, shopItem.Item.Price, shopItem.Item.ShopQuantityRemaining))
            {
                throw new ApplicationException("We are unable to perform the action :)");
            }

            shopItem.Item.ShopQuantityRemaining--;

            TokensService.CreateTransaction(TokenType.CompanyToken, profileId, shopItem.Item.Price * -1, TransactionReason.ShopItemPurchase, null, dto.DemoDate);

            var purchaseStatus = ItemRules.IsItemTypeTayra(shopItem.Item.Type) ? ShopPurchaseStatuses.Fulfilled : ShopPurchaseStatuses.PendingApproval;
            DbContext.Add(new ShopPurchase
            {
                ProfileId = profileId,
                ItemId = shopItem.ItemId,
                Status = purchaseStatus,
                ItemType = shopItem.Item.Type,
                IsFeatured = shopItem.FeaturedUntil > DateTime.UtcNow,
                IsDiscounted = shopItem.DiscountEndsAt > DateTime.UtcNow,
                Price = shopItem.Item.Price,
                PriceDiscountedFor = shopItem.Item.Price - shopItem.DiscountPrice,
                GiftFor = null,
                SegmentId = segmentId,
                LastModifiedDateId = DateHelper2.ToDateId(dto.DemoDate ?? DateTime.UtcNow)
            });

            if (purchaseStatus == ShopPurchaseStatuses.Fulfilled)
            {
                DbContext.Add(new ProfileInventoryItem
                {
                    ItemId = shopItem.ItemId,
                    ProfileId = profileId,
                    AcquireMethod = InventoryAcquireMethods.ShopPurchase,
                    IsActive = false,
                    ItemType = shopItem.Item.Type,
                    Created = dto.DemoDate ?? DateTime.UtcNow
                });
            }
            
            LogsService.LogEvent(new LogCreateDTO
            (
                eventType: LogEvents.ShopItemPurchased,
                timestamp: dto.DemoDate ?? DateTime.UtcNow,
                description: null,
                externalUrl: null,
                data: new Dictionary<string, string>
                {
                    { "itemPrice", shopItem.DiscountPrice?.ToString() ?? shopItem.Item.Price.ToString() },
                    { "itemId", shopItem.ItemId.ToString() },
                    { "itemName", shopItem.Item.Name },
                    { "purchaseStatus", purchaseStatus.ToString() },
                },
                profileId: profileId,
                shopId: shop.Id
            ));
        }

        public void EnableShopItem(Guid itemId)
        {
            var shopItem = DbContext.ShopItems.FirstOrDefault(x => x.ItemId == itemId);

            shopItem.EnsureNotNull(itemId);

            shopItem.DisabledAt = null;
        }

        public void DisableShopItem(Guid itemId)
        {
            var shopItem = DbContext.ShopItems.FirstOrDefault(x => x.ItemId == itemId);

            shopItem.EnsureNotNull(itemId);

            shopItem.DisabledAt = DateTime.UtcNow;
        }


        public void RemoveShopItem(Guid itemId)
        {
            var shopItem = DbContext.ShopItems.FirstOrDefault(x => x.ItemId == itemId);
            shopItem.EnsureNotNull(itemId);

            DbContext.Remove(shopItem);
        }
        #endregion
    }
}

//private static Expression<Func<Profile, NotificationRecipientDTO>> _segmention = x => new NotificationRecipientDTO
//{
//    FirstName = x.FirstName,
//    LastName = x.LastName,
//    ProfileId = x.Id,
//    IdentityId = x.Identity.Id,
//    EmailAddress = x.Identity.EmailAddress,
//    LocaleId = x.LocaleId
//};