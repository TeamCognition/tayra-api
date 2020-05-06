using System;
using Firdaws.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class PraiseWallController : BaseController
    {
        #region Constructor
        public PraiseWallController(OrganizationDbContext dbContext, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            DbContext = dbContext;
        }

        #endregion

        #region Properties

        protected OrganizationDbContext DbContext { get; set; }

        #endregion

        #region Action Methods

        [HttpPost("praise")]
        public IActionResult PraiseProfile([FromBody] PraiseWallPraiseDTO dto)
        {
            PraiseWallService.PraiseMember(CurrentUser.ProfileId, dto);
            DbContext.SaveChanges();

            return Ok();
        }

        [HttpPost("search")]
        public ActionResult<GridData<PraiseGridDTO>> GetPraisesGrid([FromBody] PraiseSearchGridParams gridParams)
        {
            return PraiseWallService.SearchPraises(gridParams);
        }

        #endregion
    }
}
