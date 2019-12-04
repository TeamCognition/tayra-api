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
            return Ok(ShopsService.GetShopViewDTO(1));
        }

        [HttpPost("purchaseSearch")]
        public ActionResult<GridData<ShopPurchasesGridDTO>> GetProjectCompetitions([FromBody] ShopPurchasesGridParams gridParams)
        {
            if (string.IsNullOrEmpty(gridParams.Sidx))
            {
                gridParams.Sidx = nameof(ShopPurchasesGridDTO.Created);
                gridParams.Sord = "DESC";
            }

            return ShopsService.GetShopPurchasesGridDTO(CurrentUser.ProfileId, gridParams);
        }

        [HttpPost("purchaseAdminSearch")]
        public ActionResult<GridData<ShopPurchasesAdminGridDTO>> GetProjectCompetitions([FromBody] ShopPurchasesAdminGridParams gridParams)
        {
            if (string.IsNullOrEmpty(gridParams.Sidx))
            {
                gridParams.Sidx = nameof(ShopPurchasesAdminGridDTO.Created);
                gridParams.Sord = "DESC";
            }

            return ShopsService.GetShopPurchasesAdminGridDTO(CurrentUser.ProfileId, gridParams);
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