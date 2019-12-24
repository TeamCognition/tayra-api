using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
using Firdaws.DAL;
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

        public ShopItemViewDTO GetShopItemViewDTO(int itemId)
        {
            var shopItemDto = (from si in DbContext.ShopItems
                               where si.ItemId == itemId && si.ArchivedAt == null
                               select new ShopItemViewDTO
                               {
                                   ItemId = si.ItemId,
                                   Quantity = si.Quantity,
                                   Price = si.Price,
                                   Created = si.Created,
                                   IsDisabled = si.DisabledAt.HasValue,
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
            var query = from si in DbContext.ShopItems
                        where si.Item.Name.Contains(gridParams.NameQuery) && si.ArchivedAt == null
                        where profileRole != ProfileRoles.Member || si.DisabledAt == null
                        select new ShopItemViewGridDTO
                        {
                            ItemId = si.ItemId,
                            Quantity = si.Quantity,
                            Price = si.Price,
                            Created = si.Created,
                            IsDisabled = si.DisabledAt.HasValue,
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

        public void PurchaseShopItem(int profileId, ShopItemPurchaseDTO dto)
        {
            var shop = DbContext.Shops.FirstOrDefault();
            var token = DbContext.Tokens.FirstOrDefault(x => x.Type == TokenType.CompanyToken);
            var shopItem = DbContext.ShopItems.Include(x => x.Item /*for logs*/).FirstOrDefault(x => x.ItemId == dto.ItemId && x.ArchivedAt == null);
            var profileTokenBalance = DbContext.TokenTransactions.Where(x => x.ProfileId == profileId && x.TokenId == token.Id).Sum(x => x.Value);

            shop.EnsureNotNull(shop.Id);
            shopItem.EnsureNotNull(shop.Id, dto.ItemId);

            if (!ShopRules.CanPurchaseItem(shop.ClosedAt.HasValue, profileTokenBalance, shopItem.Price, shopItem.Quantity))
            {
                throw new ApplicationException("We are unable to perform the action :)");
            }

            shopItem.Quantity--;

            TokensService.CreateTransaction(token.Id, profileId, shopItem.Price * -1, TransactionReason.ShopItemPurchase, null);

            var purchaseStatus = ItemRules.IsItemTypeTayra(shopItem.Item.Type) ? ShopPurchaseStatuses.Fulfilled : ShopPurchaseStatuses.PendingApproval;
            DbContext.Add(new ShopPurchase
            {
                ProfileId = profileId,
                ItemId = shopItem.ItemId,
                Status = purchaseStatus,
                ItemType = shopItem.Item.Type,
                IsFeatured = shopItem.FeaturedUntil > DateTime.UtcNow,
                IsDiscounted = shopItem.DiscountEndsAt > DateTime.UtcNow,
                Price = shopItem.Price,
                PriceDiscountedFor = shopItem.Price - shopItem.DiscountPrice,
                GiftFor = null,
                SegmentId = 1
            });

            if (purchaseStatus == ShopPurchaseStatuses.Fulfilled)
            {
                DbContext.Add(new ProfileInventoryItem
                {
                    ItemId = shopItem.ItemId,
                    ProfileId = profileId,
                    AcquireMethod = InventoryAcquireMethods.ShopPurchase,
                    IsActive = false,
                    ItemType = shopItem.Item.Type
                });
            }

            var buyerUsername = DbContext.Profiles.FirstOrDefault(x => x.Id == profileId).Username;
            LogsService.LogEvent(new LogCreateDTO
            {
                Event = LogEvents.ShopItemPurchased,
                Data = new Dictionary<string, string>
                {
                    { "timestamp", DateTime.UtcNow.ToString() },
                    { "profileUsername", buyerUsername },
                    { "itemPrice", shopItem.DiscountPrice?.ToString() ?? shopItem.Price.ToString() },
                    { "itemId", shopItem.ItemId.ToString() },
                    { "purchaseStatus", purchaseStatus.ToString() },
                    { "segmentId", "1" },
                    { "itemName", shopItem.Item.Name }
                },
                ProfileId = profileId,
                ShopId = shop.Id
            });
        }

        public ShopItem CreateShopItem(ShopItemCreateDTO dto)
        {
            return
                DbContext.Add(new ShopItem
                {
                    Price = dto.Price,
                    Quantity = dto.Quantity,
                    Item = new Item
                    {
                        Name = dto.Name,
                        Description = dto.Description,
                        Image = dto.Image,
                        IsActivable = dto.IsActivable,
                        IsDisenchantable = dto.IsDisenchantable,
                        IsGiftable = dto.IsGiftable,
                        Type = dto.Type,
                        Rarity = dto.Rarity,
                        WorthValue = (float)Math.Round(dto.Price * 0.90f, 2)
                    }
                }).Entity;
        }

        public ShopItem UpdateShopItem(ShopItemUpdateDTO dto)
        {
            var shopItem = DbContext.ShopItems.Include(x => x.Item).FirstOrDefault(x => x.ItemId == dto.ItemId);

            shopItem.EnsureNotNull(dto.ItemId);

            shopItem.Price = dto.Price;
            shopItem.Quantity = dto.Quantity;

            var item = dto.AffectOwnedItems ? shopItem.Item : new Item();
            shopItem.Item = item;

            item.Name = dto.Name;
            item.Description = dto.Description;
            item.Image = dto.Image;
            item.IsActivable = dto.IsActivable;
            item.IsDisenchantable = dto.IsDisenchantable;
            item.IsGiftable = dto.IsGiftable;
            item.Type = dto.Type;
            item.Rarity = dto.Rarity;
            item.WorthValue = (float)Math.Round(dto.Price * 0.90f, 2);

            return shopItem;
        }

        public void EnableShopItem(int itemId)
        {
            var shopItem = DbContext.ShopItems.FirstOrDefault(x => x.ItemId == itemId);

            shopItem.EnsureNotNull(itemId);

            shopItem.DisabledAt = null;
        }

        public void DisableShopItem(int itemId)
        {
            var shopItem = DbContext.ShopItems.FirstOrDefault(x => x.ItemId == itemId);

            shopItem.EnsureNotNull(itemId);

            shopItem.DisabledAt = DateTime.UtcNow;
        }

        public void RemoveShopItem(int itemId)
        {
            var shopItem = DbContext.ShopItems.FirstOrDefault(x => x.ItemId == itemId);

            shopItem.EnsureNotNull(itemId);

            shopItem.ArchivedAt = DateTime.UtcNow;
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