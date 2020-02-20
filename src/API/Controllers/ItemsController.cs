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
            return Ok(ItemsService.GetGridData(CurrentUser.Role, gridParams));
        }

        [HttpPost("create")]
        public ActionResult<IDTO> CreateItem([FromBody] ItemCreateDTO dto)
        {
            var item = ItemsService.CreateItem(dto);
            OrganizationContext.SaveChanges();

            return Ok(new { Id = item.Id});
        }

        [HttpPut("update")]
        public ActionResult<IDTO> UpdateItem([FromBody] ItemUpdateDTO dto)
        {
            var item = ItemsService.UpdateItem(dto);
            OrganizationContext.SaveChanges();

            return Ok(new { Id = item.Id });
        }

        #endregion
    }
}