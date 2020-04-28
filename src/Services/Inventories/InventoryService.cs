using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
using Firdaws.DAL;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Mailer;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class InventoryService : BaseService<OrganizationDbContext>, IInventoriesService
    {
        #region Constructor
        protected ILogsService LogsService { get; set; }

        protected ITokensService TokensService { get; set; }

        public InventoryService(ILogsService logsService, ITokensService tokensService, OrganizationDbContext dbContext) : base(dbContext)
        {
            LogsService = logsService;
            TokensService = tokensService;
        }

        #endregion

        #region Public Methods

        public InventoryItemViewDTO GetInventoryItemViewDTO(int inventoryItemId)
        {
            var invItemDto = (from pi in DbContext.ProfileInventoryItems
                              where pi.Id == inventoryItemId
                              select new InventoryItemViewDTO
                              {
                                  Created = pi.Created,
                                  AcquireMethod = pi.AcquireMethod,
                                  AcquireDetail = pi.AcquireDetail,
                                  Name = pi.Item.Name,
                                  Description = pi.Item.Description,
                                  Image = pi.Item.Image,
                                  Price = pi.Item.Price,
                                  IsActivable = pi.Item.IsActivable,
                                  IsDisenchantable = pi.Item.IsDisenchantable,
                                  IsGiftable = pi.Item.IsGiftable,
                                  IsActive = pi.IsActive,
                                  Type = pi.Item.Type,
                                  Rarity = pi.Item.Rarity
                              }).FirstOrDefault();

            invItemDto.EnsureNotNull(inventoryItemId);

            return invItemDto;
        }

        public GridData<InventoryItemGridDTO> GetInventoryItemViewGridDTO(InventoryItemGridParams gridParams)
        {
            if(string.IsNullOrEmpty(gridParams.ProfileUsername))
            {
                throw new ApplicationException(nameof(gridParams.ProfileUsername) + "must be provided");
            }

            var profile = DbContext.Profiles.FirstOrDefault(x => x.Username == gridParams.ProfileUsername);

            profile.EnsureNotNull(gridParams.ProfileUsername);

            IQueryable<ProfileInventoryItem> scope = DbContext.ProfileInventoryItems;

            var query = from i in scope
                        where i.ProfileId == profile.Id
                        where i.Item.Name.Contains(gridParams.ItemNameQuery)
                        where !i.ClaimRequired || i.ClaimedAt.HasValue
                        select new InventoryItemGridDTO
                        {
                            InventoryItemId = i.Id,
                            ItemId = i.ItemId,
                            Created = i.Created,
                            Name = i.Item.Name,
                            Description = i.Item.Description,
                            Image = i.Item.Image,
                            IsActive = i.IsActive,
                            Type = i.Item.Type,
                            Rarity = i.Item.Rarity,
                            IsActivable = i.Item.IsActivable,
                            IsDisenchantable = i.Item.IsDisenchantable,
                            IsGiftable = i.Item.IsGiftable,
                            Price = i.Item.Price
                        };

            GridData<InventoryItemGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public void Activate(int profileId, InventoryItemActivateToggleDTO dto)
        {
            var itemToActivate = DbContext.ProfileInventoryItems.Include(x => x.Item).FirstOrDefault(x => x.Id == dto.InventoryItemId && x.ProfileId == profileId);

            itemToActivate.EnsureNotNull(profileId, dto.InventoryItemId);

            if (!InventoryRules.CanActivateInventoryItem(profileId, itemToActivate, itemToActivate.Item))
            {
                throw new ApplicationException("We are unable to perform this action :)");
            }

            if (itemToActivate.Item.Type == ItemTypes.TayraTitle)
            {
                var currentActive = DbContext.ProfileInventoryItems.FirstOrDefault(x => x.ItemType == itemToActivate.Item.Type && x.ProfileId == profileId && x.IsActive);
                if (currentActive != null)
                {
                    currentActive.IsActive = false;
                }

                itemToActivate.IsActive = true;
            }
            else if (itemToActivate.Item.Type == ItemTypes.TayraBadge)
            {
                var currentActiveCount = DbContext.ProfileInventoryItems.Where(x => x.ItemType == itemToActivate.Item.Type && x.ProfileId == profileId && x.IsActive).Count();
                if (currentActiveCount >= 3)
                {
                    throw new ApplicationException("You already have 3 active badges");
                }

                itemToActivate.IsActive = true;
            }
        }

        public void Deactivate(int profileId, InventoryItemActivateToggleDTO dto)
        {
            var invItem = DbContext.ProfileInventoryItems.FirstOrDefault(x => x.ProfileId == profileId && x.Id == dto.InventoryItemId);

            invItem.EnsureNotNull(dto.InventoryItemId, profileId);

            invItem.IsActive = false;
        }

        public void Gift(int profileId, InventoryItemGiftDTO dto)
        {
            var invItem = DbContext.ProfileInventoryItems.Include(x => x.Item).FirstOrDefault(x => x.Id == dto.InventoryItemId);

            invItem.EnsureNotNull(dto.InventoryItemId);

            if (!InventoryRules.CanGiftInventoryItem(profileId, dto.ReceiverId, invItem.ProfileId, invItem.Item.IsGiftable, invItem.IsActive))
            {
                throw new ApplicationException("We are unable to perform this action :)");
            }

            DbContext.ProfileInventoryItems.Remove(invItem);
            var giftedItem = DbContext.Add(new ProfileInventoryItem
            {
                ItemId = invItem.ItemId,
                ProfileId = dto.ReceiverId,
                AcquireMethod = InventoryAcquireMethods.MemberGift,
                ClaimRequired = dto.ClaimRequired,
                ItemType = invItem.Item.Type,
                Created = dto.DemoDate ?? DateTime.UtcNow
            }).Entity;

            DbContext.Add(new ItemGift
            {
                ItemId = invItem.ItemId,
                ReceiverId = dto.ReceiverId,
                SenderId = profileId,
                Created = dto.DemoDate ?? DateTime.UtcNow
            });

            if (dto.ClaimRequired)
            {
                DbContext.GetTrackedClaimBundle(dto.ReceiverId, ClaimBundleTypes.Gift).AddItems(giftedItem);
            }

            var gifterUsername = DbContext.Profiles.FirstOrDefault(x => x.Id == profileId).Username;
            var receiverUsername = DbContext.Profiles.FirstOrDefault(x => x.Id == dto.ReceiverId).Username;
            LogsService.LogEvent(new LogCreateDTO
            {
                Event = LogEvents.InventoryItemGifted,
                Data = new Dictionary<string, string>
                {
                    { "timestamp", (dto.DemoDate ?? DateTime.UtcNow).ToString() },
                    { "profileUsername", gifterUsername },
                    { "receiverUsername", receiverUsername },
                    { "itemName", invItem.Item.Name }
                },
                ProfileId = profileId,
            });

            LogsService.LogEvent(new LogCreateDTO
            {
                Event = LogEvents.InventoryItemReceived,
                Data = new Dictionary<string, string>
                {
                    { "timestamp", (dto.DemoDate ?? DateTime.UtcNow).ToString() },
                    { "profileUsername", receiverUsername },
                    { "gifterUsername", gifterUsername },
                    { "itemName", invItem.Item.Name }
                },
                ProfileId = dto.ReceiverId,
            });

            LogsService.SendLog(dto.ReceiverId, LogEvents.InventoryItemGifted, new EmailGiftReceivedDTO(gifterUsername));
        }

        public void Disenchant(int profileId, InventoryItemDisenchantDTO dto)
        {
            var invItem = DbContext.ProfileInventoryItems.Include(x => x.Item).Include(x => x.Profile).FirstOrDefault(x => x.ProfileId == profileId && x.Id == dto.InventoryItemId);

            invItem.EnsureNotNull(profileId, dto.InventoryItemId);

            DbContext.Add(new ItemDisenchant
            {
                ItemId = invItem.ItemId,
                ProfileId = profileId
            });

            var disenchantValue = Math.Round(invItem.Item.Price * 0.90, 2);
            LogsService.LogEvent(new LogCreateDTO
            {
                Event = LogEvents.InventoryItemDisenchanted,
                Data = new Dictionary<string, string>
                {
                    { "profileId", profileId.ToString() },
                    { "profileUsername", invItem.Profile.Username },
                    { "itemId", invItem.ItemId.ToString() },
                    { "ItemName", invItem.Item.Name},
                    { "disenchantValue", disenchantValue.ToString() },
                    { "timestamp", DateTime.UtcNow.ToString() }
                },

                ProfileId = profileId,
            });

            DbContext.Remove(invItem);

            TokensService.CreateTransaction(TokenType.CompanyToken, profileId, disenchantValue, TransactionReason.ItemDisenchant, null);
        }

        public void Give(int profileId, InventoryGiveDTO dto)
        {
            var item = DbContext.Items.FirstOrDefault(x => x.Id == dto.ItemId);
            var receiverId = DbContext.Profiles.Where(x => x.Username == dto.ReceiverUsername).Select(x => x.Id).FirstOrDefault();
            var gifterUsername = DbContext.Profiles.Where(x => x.Id == profileId).Select(x => x.Username).FirstOrDefault();

            item.EnsureNotNull(dto.ItemId);

            var givenItem = DbContext.Add(new ProfileInventoryItem
            {
                ItemId = item.Id,
                ProfileId = receiverId,
                ItemType = item.Type,
                AcquireMethod = InventoryAcquireMethods.ManagerGift,
                ClaimRequired = dto.ClaimRequired
            }).Entity;

            if (dto.ClaimRequired)
            {
                DbContext.GetTrackedClaimBundle(receiverId, ClaimBundleTypes.GiftFromAdmin).AddItems(givenItem);
            }
            LogsService.SendLog(receiverId, LogEvents.InventoryItemGifted, new EmailGiftReceivedDTO(gifterUsername));
        }
        #endregion
    }
}
