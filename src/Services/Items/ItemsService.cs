using System;
using System.Linq;
using Firdaws.Core;
using Firdaws.DAL;
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
                                  Name = i.Name,
                                  Description = i.Description,
                                  Image = i.Image,
                                  WorthValue = i.WorthValue,
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
                            WorthValue = i.WorthValue,
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

        #endregion
    }
}