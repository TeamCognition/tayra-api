using System;
using Firdaws.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class IdentitiesController : BaseController
    {
        #region Constructor

        public IdentitiesController(OrganizationDbContext dbContext, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            DbContext = dbContext;
        }

        #endregion

        #region Properties

        protected OrganizationDbContext DbContext;

        #endregion

        #region Action Methods

        [HttpPost("create")]
        public ActionResult Create([FromBody] IdentityCreateDTO dto)
        {
            IdentitiesService.Create(dto);
            DbContext.SaveChanges();

            return Ok();
        }

        [HttpPost("searchEmails")]
        public ActionResult<GridData<IdentityEmailsGridDTO>> GetIdentityEmails([FromBody] IdentityEmailsGridParams gridParams)
        {
            if (string.IsNullOrEmpty(gridParams.Sidx))
            {
                gridParams.Sidx = nameof(IdentityEmailsGridDTO.AddedOn);
                gridParams.Sord = "DESC";
            }

            return IdentitiesService.GetIdentityEmailsGridData(CurrentUser.Id, gridParams);
        }

        [HttpPost("addEmail")]
        public ActionResult AddEmail([FromQuery] string email)
        {
            IdentitiesService.AddEmail(CurrentUser.IdentityId, email);
            return Ok();
        }

        [HttpPost("makeEmailPrimary")]
        public ActionResult MakeEmailPrimary([FromQuery] string email)
        {
            IdentitiesService.SetPrimaryEmail(CurrentUser.IdentityId, email);
            return Ok();
        }

        [HttpDelete("removeEmail")]
        public ActionResult RemoveEmail([FromQuery] string email)
        {
            IdentitiesService.RemoveEmail(CurrentUser.IdentityId, email);
            return Ok();
        }

        #endregion
    }
}