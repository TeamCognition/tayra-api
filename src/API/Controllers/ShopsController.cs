using System;
using Firdaws.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class ShopsController : BaseController
    {
        #region Constructor

        public ShopsController(IServiceProvider serviceProvider, OrganizationDbContext dbContext) : base(serviceProvider)
        {
            OrganizationContext = dbContext;
        }

        #endregion

        protected OrganizationDbContext OrganizationContext;

        #region Action Methods

        [HttpGet]
        public ActionResult<ShopViewDTO> GetShop()
        {
            return Ok(ShopsService.GetShopViewDTO());
        }

        [HttpPost("purchaseSearch")]
        public ActionResult<GridData<ShopPurchasesGridDTO>> GetShopPurchasesSearch([FromBody] ShopPurchasesGridParams gridParams)
        {
            return ShopsService.GetShopPurchasesGridDTO(CurrentUser.ProfileId, CurrentUser.Role, gridParams);
        }

        [HttpPost("close")]
        public IActionResult CloseShop()
        {
            ShopsService.CloseShop(1);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("open")]
        public IActionResult OpenShop()
        {
            ShopsService.OpenShop(1);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPut("purchaseStatus/{shopPurchaseId:int}")]
        public IActionResult UpdateShopItem([FromRoute] int shopPurchaseId, [FromBody] ShopPurchaseStatuses status)
        {
            ShopsService.UpdateShopPurchaseStatus(CurrentUser.ProfileId, shopPurchaseId, status);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}