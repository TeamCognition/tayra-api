using System;
using Firdaws.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class ItemsController : BaseController
    {
        #region Constructor

        public ItemsController(IServiceProvider serviceProvider, OrganizationDbContext dbContext) : base(serviceProvider)
        {
            OrganizationContext = dbContext;
        }

        #endregion

        protected OrganizationDbContext OrganizationContext;

        #region Action Methods

        [HttpGet("{itemId:int}")]
        public ActionResult<ItemViewDTO> GetItem([FromRoute]int itemId)
        {
            return Ok(ItemsService.GetItemViewDTO(CurrentUser.Role, itemId));
        }

        [HttpPost("search")]
        public ActionResult<GridData<ItemGridDTO>> GetInventoryItemViewGridData([FromBody] ItemGridParams gridParams)
        {
            if (string.IsNullOrEmpty(gridParams.Sidx))
            {
                gridParams.Sidx = nameof(InventoryItemGridDTO.Created);
                gridParams.Sord = "DESC";
            }


            return Ok(ItemsService.GetGridData(CurrentUser.Role, gridParams));
        }

        #endregion
    }
}