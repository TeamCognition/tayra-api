using System;
using Firdaws.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class InventoryItemsController : BaseController
    {
        #region Constructor

        public InventoryItemsController(IServiceProvider serviceProvider, OrganizationDbContext dbContext) : base(serviceProvider)
        {
            OrganizationContext = dbContext;
        }

        #endregion

        protected OrganizationDbContext OrganizationContext;

        #region Action Methods

        [HttpGet("{inventoryItemId:int}")]
        public ActionResult<InventoryItemViewDTO> GetInventoryItem([FromRoute]int inventoryItemId)
        {
            return Ok(InventoriesService.GetInventoryItemViewDTO(inventoryItemId));
        }

        [HttpPost("search")]
        public ActionResult<GridData<InventoryItemGridDTO>> GetInventoryItemViewGridData([FromBody] InventoryItemGridParams gridParams)
        {
            return InventoriesService.GetInventoryItemViewGridDTO(gridParams);
        }

        [HttpPost("activate")]
        public IActionResult ActivateInventoryItem([FromBody] InventoryItemActivateToggleDTO dto)
        {
            InventoriesService.Activate(CurrentUser.ProfileId, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("deactivate")]
        public IActionResult DeactivateInventoryItem([FromBody] InventoryItemActivateToggleDTO dto)
        {
            InventoriesService.Deactivate(CurrentUser.ProfileId, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("gift")]
        public IActionResult GiftInventoryItem([FromBody] InventoryItemGiftDTO dto)
        {
            InventoriesService.Gift(CurrentUser.ProfileId, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("disenchant")]
        public IActionResult DisenchantInventoryItem([FromBody] InventoryItemDisenchantDTO dto)
        {
            InventoriesService.Disenchant(CurrentUser.ProfileId, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("give")]
        public IActionResult GiveInventoryItem([FromBody] InventoryGiveDTO dto)
        {
            InventoriesService.Give(CurrentUser.ProfileId,dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}