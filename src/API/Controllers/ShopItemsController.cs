using System;
using Cog.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class ShopItemsController : BaseController
    {
        #region Constructor

        public ShopItemsController(IServiceProvider serviceProvider, OrganizationDbContext dbContext) : base(serviceProvider)
        {
            OrganizationContext = dbContext;
        }

        #endregion

        protected OrganizationDbContext OrganizationContext;

        #region Action Methods

        [HttpGet("{itemId:int}")]
        public ActionResult<ShopItemViewDTO> GetShopItem([FromRoute]int itemId)
        {
            return Ok(ShopItemsService.GetShopItemViewDTO(itemId));
        }

        [HttpPost("search")]
        public ActionResult<GridData<ShopItemViewGridDTO>> GetShopItemsViewGrid([FromBody] ShopItemViewGridParams gridParams)
        {
            return ShopItemsService.GetShopItemViewGridDTO(CurrentUser.Role, gridParams);
        }

        [HttpPost("purchase")]
        public IActionResult PurchaseShopItem([FromBody] ShopItemPurchaseDTO dto)
        {
            ShopItemsService.PurchaseShopItem(CurrentUser.ProfileId, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("disable")]
        public IActionResult DisableShopItem([FromBody] ShopItemEnabledToggleDTO dto)
        {
            ShopItemsService.DisableShopItem(dto.ItemId);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("enable")]
        public IActionResult EnabledShopItem([FromBody] ShopItemEnabledToggleDTO dto)
        {
            ShopItemsService.EnableShopItem(dto.ItemId);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}