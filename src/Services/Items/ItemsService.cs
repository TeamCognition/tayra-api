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

            var shopItem = DbContext.ShopItems.Where(x => x.ItemId == itemId).FirstOrDefault();
            if(shopItem != null)
            {
                itemDto.ShopRemainingQuantity = shopItem.QuantityReservedRemaining;
                itemDto.PlaceInShop = true;
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
                        QuantityChange = dto.Quantity.Value
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

            //Is this even needed?
            if(dto.Quantity.HasValue && item.Reservations.Sum(x => x.QuantityChange) != dto.Quantity)
            {
                DbContext.Add(new ItemReservation
                {
                    ItemId = item.Id,
                    QuantityChange = dto.Quantity.Value - item.Reservations.Sum(x => x.QuantityChange)
                });
            }
            else if (!dto.Quantity.HasValue)
            {
                DbContext.RemoveRange(item.Reservations);
            }

            var shopItem = DbContext.ShopItems.Include(x => x.Item).FirstOrDefault(x => x.ItemId == dto.ItemId);

            if(!dto.PlaceInShop)
            {
                if (shopItem != null)
                {
                    DbContext.Remove(shopItem);
                }
                return item;
            }
            
            shopItem = shopItem ?? DbContext.Add(new ShopItem { ItemId = item.Id, IsGlobal = true}).Entity;

            if((!dto.ShopQuantity.HasValue && dto.Quantity.HasValue)
            || dto.Quantity < (dto.ShopQuantity ?? 0 - shopItem.QuantityReservedRemaining ?? 0)) //checks if there is enough quantity now
            {
                throw new ApplicationException("shop quantity exceeds item quantity"); 
            }

            //         new quantity - shopNewQuantity -        shopCurrentRemainingQuantity -             currentQuantity
            var newQ = dto.Quantity - (dto.ShopQuantity ?? 0 - shopItem.QuantityReservedRemaining ?? 0) - item?.Reservations.Sum(x => x.QuantityChange);
            if (dto.Quantity.HasValue && newQ != dto.Quantity)
            {
                item.Reservations.Add(DbContext.Add(new ItemReservation
                {
                    ItemId = item.Id,
                    QuantityChange = dto.Quantity.Value - item.Reservations.Sum(x => x.QuantityChange)
                }).Entity);
            }
            if (!dto.Quantity.HasValue)
            {
                DbContext.RemoveRange(item.Reservations);
            }

            shopItem.QuantityReservedRemaining = dto.ShopQuantity;
            if(!dto.AffectOwnedItems)
            {
                DbContext.Remove(shopItem.Item);
                shopItem.Item = new Item();
            }

            shopItem.Item.Price = dto.Price;
            shopItem.Item.IsQuantityLimited = dto.Quantity.HasValue;
            shopItem.Item.Name = dto.Name;
            shopItem.Item.Description = dto.Description;
            shopItem.Item.Image = dto.Image;
            shopItem.Item.IsActivable = dto.IsActivable;
            shopItem.Item.IsDisenchantable = dto.IsDisenchantable;
            shopItem.Item.IsGiftable = dto.IsGiftable;
            shopItem.Item.Type = dto.Type;
            shopItem.Item.Rarity = dto.Rarity;

            return shopItem.Item;
        }

        #endregion
    }
}