using System;
using Firdaws.Core;
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
            if (string.IsNullOrEmpty(gridParams.Sidx))
            {
                gridParams.Sidx = nameof(ShopItemViewGridDTO.Created);
                gridParams.Sord = "DESC";
            }

            return ShopItemsService.GetShopItemViewGridDTO(CurrentUser.Role, gridParams);
        }

        [HttpPost("purchase")]
        public IActionResult PurchaseShopItem([FromBody] ShopItemPurchaseDTO dto)
        {
            ShopItemsService.PurchaseShopItem(CurrentUser.Id, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("create")]
        public ActionResult<IDTO> CreateShopItem([FromBody] ShopItemCreateDTO dto)
        {
            var shopItem = ShopItemsService.CreateShopItem(dto);
            OrganizationContext.SaveChanges();

            return Ok(new { Id = shopItem.ItemId });
        }

        [HttpPut("update")]
        public ActionResult<IDTO> UpdateShopItem([FromBody] ShopItemUpdateDTO dto)
        {
            var shopItem = ShopItemsService.UpdateShopItem(dto);
            OrganizationContext.SaveChanges();

            return Ok(new { Id = shopItem.ItemId });
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

        [HttpDelete("{itemId:int}")] 
        public IActionResult RemoveShopItem([FromRoute] int itemId)
        {
            ShopItemsService.RemoveShopItem(itemId);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}