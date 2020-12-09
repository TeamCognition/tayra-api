using System;
using Cog.Core;
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
            return ShopsService.GetShopViewDTO(CurrentUser.ProfileId, CurrentUser.Role);
        }

        [HttpPost("purchaseSearch")]
        public ActionResult<GridData<ShopPurchasesGridDTO>> GetShopPurchasesSearch([FromBody] ShopPurchasesGridParams gridParams)
        {
            return ShopsService.GetShopPurchasesGridDTO(CurrentUser.ProfileId, CurrentUser.Role, gridParams);
        }

        [HttpPost("close")]
        public IActionResult CloseShop()
        {
            ShopsService.CloseShop();
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("open")]
        public IActionResult OpenShop()
        {
            ShopsService.OpenShop();
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPut("purchaseStatus/{shopPurchaseId:int}")]
        public IActionResult UpdateShopItem([FromRoute] Guid shopPurchaseId, [FromBody] ShopPurchaseStatuses status)
        {
            ShopsService.UpdateShopPurchaseStatus(CurrentUser.ProfileId, shopPurchaseId, status);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpGet("tokenAverageEarnings")]
        public ActionResult<ShopTokenAverageEarningsDTO> GetTokenWeeklyAverageEarnings()
        {
            return ShopsService.GetTokenAverageEarnings();
        }

        #endregion
    }
}