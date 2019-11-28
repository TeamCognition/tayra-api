﻿using System;
using Firdaws.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class IdentitiesController : BaseController
    {
        #region Constructor

        public IdentitiesController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        #endregion

        #region Properties

        #endregion

        #region Action Methods

        [AllowAnonymous, HttpPost("create")]
        public ActionResult Create([FromBody] IdentityCreateDTO dto)
        {
            //var o = Resolve<IOrganizationsService>();
            //o.Create(new OrganizationCreateDTO
            //{
            //    Key = "localhost:3000",
            //    Name = "Localhost 3000",
            //    Timezone = "Europe Central",
            //    DatabaseServer = "sqlserver-tayra.database.windows.net",
            //    DatabaseName = "sqldb-tayra-tenants_free-prod",
            //    TemplateConnectionString = "User ID = tyradmin; Password = Kr7N9#p!2AbR;Connect Timeout=100;Application Name=Tayra"
            //});

            IdentitiesService.InternalCreateWithProfile(dto);
            //DbContext.SaveChanges();

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