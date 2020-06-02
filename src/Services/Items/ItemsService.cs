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
                               ShopQuantityRemaining = i.ShopQuantityRemaining,
                               ChallengesQuantityRemaining = i.ChallengesQuantityRemaining,
                               GiveawayQuantityRemaining = i.GiveawayQuantityRemaining,
                               Created = i.Created,
                               LastModified = i.LastModified
                           }).FirstOrDefault();

            itemDto.EnsureNotNull(itemId);

            var shopItem = DbContext.ShopItems.Where(x => x.ItemId == itemId).FirstOrDefault();
            if(shopItem != null)
            {
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
                            ShopQuantityRemaining = i.ShopQuantityRemaining,
                            ChallengesQuantityRemaining = i.ChallengesQuantityRemaining,
                            GiveawayQuantityRemaining = i.GiveawayQuantityRemaining,
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
                CreatedDateId = DateHelper2.ToDateId(DateTime.UtcNow),
                ShopQuantityRemaining = dto.ShopQuantityRemaining,
                ChallengesQuantityRemaining = dto.ChallengesQuantityRemaining,
                GiveawayQuantityRemaining = dto.GiveawayQuantityRemaining
            }).Entity;

            if(dto.PlaceInShop)
            {
                DbContext.Add(new ShopItem
                {
                    Item = item,
                    IsGlobal = true
                });
            }

            return item;
        }

        public Item UpdateItem(ItemUpdateDTO dto)
        {
            var item = DbContext.Items.FirstOrDefault(x => x.Id == dto.ItemId);

            item.EnsureNotNull(dto.ItemId);

            if (!dto.AffectOwnedItems)
            {
                DbContext.Remove(item);
                item = new Item();
            }

            item.Price = dto.Price;
            item.Name = dto.Name;
            item.Description = dto.Description;
            item.Image = dto.Image;
            item.IsActivable = dto.IsActivable;
            item.IsDisenchantable = dto.IsDisenchantable;
            item.IsGiftable = dto.IsGiftable;
            item.Type = dto.Type;
            item.Rarity = dto.Rarity;

            item.ShopQuantityRemaining = dto.ShopQuantityRemaining;
            item.ChallengesQuantityRemaining = dto.ChallengesQuantityRemaining;
            item.GiveawayQuantityRemaining = dto.GiveawayQuantityRemaining;

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

            return shopItem.Item;
        }
        public void Archive(int itemId)
        {
            var item = DbContext.Items.FirstOrDefault(x => x.Id == itemId);
            var shopItem = DbContext.ShopItems.FirstOrDefault(x => x.Id == itemId);

            item.EnsureNotNull(itemId);
            shopItem.EnsureNotNull(itemId);

            DbContext.Remove(item);
        }

        public void DeleteItem(int itemId)
        {
            var item = DbContext.Items.FirstOrDefault(x => x.Id == itemId);
            item.EnsureNotNull(itemId);

            DbContext.Remove(item);

            var shopItem = DbContext.ShopItems.FirstOrDefault(x => x.ItemId == itemId);
            if(shopItem != null)
            {
                DbContext.Remove(shopItem);
            }
        }

        #endregion
    }
}