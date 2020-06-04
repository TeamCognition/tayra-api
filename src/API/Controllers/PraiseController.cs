using System;
using Cog.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class PraiseController : BaseController
    {
        #region Constructor
        public PraiseController(OrganizationDbContext dbContext, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            DbContext = dbContext;
        }

        #endregion

        #region Properties

        protected OrganizationDbContext DbContext { get; set; }

        #endregion

        #region Action Methods

        [HttpPost]
        public IActionResult PraiseProfile([FromBody] PraiseProfileDTO dto)
        {
            PraiseService.PraiseProfile(CurrentUser.ProfileId, dto);
            DbContext.SaveChanges();

            return Ok();
        }

        [HttpPost("search")]
        public ActionResult<GridData<PraiseSearchGridDTO>> GetPraisesGrid([FromBody] PraiseSearchGridParams gridParams)
        {
            return PraiseService.SearchPraises(gridParams);
        }

        #endregion
    }
}
