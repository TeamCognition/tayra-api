using System;
using System.Linq;
using Firdaws.Core;
using Firdaws.DAL;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class ItemsService : BaseService<OrganizationDbContext>, IItemsService
    {
        #region Constructor

        public ItemsService(OrganizationDbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        public ItemViewDTO GetItemViewDTO(ProfileRoles role, int itemId)
        {
            if (role == ProfileRoles.Member)
            {
                throw new ApplicationException("Members don't have access to this API");
            }

            var itemDto = (from i in DbContext.Items
                           where i.Id == itemId
                           select new ItemViewDTO
                           {
                               ItemId = i.Id,
                               Name = i.Name,
                               Description = i.Description,
                               Image = i.Image,
                               Price = i.Price,
                               IsActivable = i.IsActivable,
                               IsDisenchantable = i.IsDisenchantable,
                               IsGiftable = i.IsGiftable,
                               Type = i.Type,
                               Rarity = i.Rarity,
                               Quantity = i.IsQuantityLimited ? i.Reservations.Sum(x => x.QuantityChange) : (int?)null,
                               Created = i.Created,
                               LastModified = i.LastModified
                           }).FirstOrDefault();

            itemDto.EnsureNotNull(itemId);

            var shopItem = DbContext.ShopItems.Where(x => x.Id == itemId).FirstOrDefault();
            if(shopItem != null)
            {
                itemDto.ShopRemainingQuantity = shopItem.QuantityReservedRemaining;
                itemDto.PlaceInShop = true;
                itemDto.IsDisabled = shopItem.DisabledAt.HasValue;
            }

            return itemDto; 
        }

        public GridData<ItemGridDTO> GetGridData(ProfileRoles role, ItemGridParams gridParams)
        {
            if(role == ProfileRoles.Member)
            {
                throw new ApplicationException("Members don't have access to this API");
            }

            if(gridParams.Sidx == nameof(ItemGridDTO.Quantity))
            {
                throw new ApplicationException("Can't order by quantity for now");
            }

            IQueryable<Item> scope = DbContext.Items;

            var query = from i in scope
                        select new ItemGridDTO
                        {
                            ItemId = i.Id,
                            Name = i.Name,
                            Description = i.Description,
                            Image = i.Image,
                            Price = i.Price,
                            IsActivable = i.IsActivable,
                            IsDisenchantable = i.IsDisenchantable,
                            IsGiftable = i.IsGiftable,
                            Type = i.Type,
                            Rarity = i.Rarity,
                            Quantity = i.IsQuantityLimited ? i.Reservations.Sum(x => x.QuantityChange) : (int?)null,
                            Created = i.Created,
                            LastModified = i.LastModified
                        };

            GridData<ItemGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public Item CreateItem(ItemCreateDTO dto)
        {
            if (ItemRules.IsShopQuantityExceedingItems(dto.Quantity, dto.ShopQuantity))
            {
                throw new ApplicationException("shop quantity exceeds item quantity");
            }

            var item = DbContext.Add(new Item
            {
                Name = dto.Name,
                Description = dto.Description,
                Image = dto.Image,
                IsActivable = dto.IsActivable,
                IsDisenchantable = dto.IsDisenchantable,
                IsGiftable = dto.IsGiftable,
                Type = dto.Type,
                Rarity = dto.Rarity,
                Price = dto.Price,
                IsQuantityLimited = dto.Quantity.HasValue,
                Reservations = !dto.Quantity.HasValue ? null : new ItemReservation[]
                {
                    new ItemReservation
                    {
                        QuantityChange = dto.Quantity.Value - dto.ShopQuantity ?? 0
                    }
                }
            }).Entity;

            if(dto.PlaceInShop)
            {
                DbContext.Add(new ShopItem
                {
                    Item = item,
                    QuantityReservedRemaining = dto.ShopQuantity,
                    IsGlobal = true
                });
            }

            return item;
        }

        public Item UpdateItem(ItemUpdateDTO dto)
        {
            var item = DbContext.Items.Include(x => x.Reservations).FirstOrDefault(x => x.Id == dto.ItemId);

            item.EnsureNotNull(dto.ItemId);

            if (!dto.AffectOwnedItems)
            {
                DbContext.Remove(item);
                item = new Item();
            }

            var currentAvailableQuantity = item?.Reservations.Sum(x => x.QuantityChange) ?? 0;

            var newQ = dto.Quantity - (dto.ShopQuantity ?? 0) - currentAvailableQuantity;
            if (dto.Quantity.HasValue && newQ != currentAvailableQuantity)
            {
                item.Reservations.Add(DbContext.Add(new ItemReservation
                {
                    ItemId = item.Id,
                    QuantityChange = newQ.Value
                }).Entity);
            }
            if (!dto.Quantity.HasValue)
            {
                DbContext.RemoveRange(item.Reservations);
            }

            item.Price = dto.Price;
            item.IsQuantityLimited = dto.Quantity.HasValue;
            item.Name = dto.Name;
            item.Description = dto.Description;
            item.Image = dto.Image;
            item.IsActivable = dto.IsActivable;
            item.IsDisenchantable = dto.IsDisenchantable;
            item.IsGiftable = dto.IsGiftable;
            item.Type = dto.Type;
            item.Rarity = dto.Rarity;

            var shopItem = DbContext.ShopItems.Include(x => x.Item).FirstOrDefault(x => x.ItemId == dto.ItemId);
            
            if (!dto.PlaceInShop)
            {
                if (shopItem != null)
                {
                    DbContext.Remove(shopItem);
                }
                return item;
            }

            shopItem = shopItem ?? DbContext.Add(new ShopItem { Item = item, IsGlobal = true }).Entity;

            if (ItemRules.IsShopQuantityExceedingItems(dto.Quantity, dto.ShopQuantity))
            {
                throw new ApplicationException("shop quantity exceeds item quantity");
            }

            shopItem.QuantityReservedRemaining = dto.ShopQuantity;

            return shopItem.Item;
        }

        #endregion
    }
}