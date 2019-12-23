using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Tayra.API.Helpers;
using Tayra.Common;
using Tayra.Connectors.Common;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class IntegrationsController : BaseDataController
    {
        #region Constructor

        public IntegrationsController(IServiceProvider serviceProvider, IConnectorResolver connectorResolver, OrganizationDbContext context) : base(serviceProvider, context)
        {
            ConnectorResolver = connectorResolver;
        }

        #endregion

        public IConnectorResolver ConnectorResolver { get; }

        #region Public Methods

        [HttpGet, Route("")]
        public ActionResult<List<IntegrationProjectViewDTO>> Get()
        {
            return IntegrationsService.GetProjectIntegrations(CurrentProject.Id);
        }

        [HttpGet, Route("connect/{type?}")]
        public IActionResult Connect(IntegrationType type)
        {
            var connector = ConnectorResolver.Get<IOAuthConnector>(type);
            return Redirect(connector.GetAuthUrl(Cipher.Encrypt($"{CurrentUser.ProfileId}|{CurrentUser.Role}|{CurrentProject.Id}")));
        }

        [HttpGet, Route("settings/atj")]
        public ActionResult<JiraSettingsViewDTO> GetJiraSettings()
        {
            return IntegrationsService.GetJiraSettingsViewDTO(CurrentProject.Id);
        }

        [HttpPost, Route("settings/atj")]
        public ActionResult SetJiraSettings([FromBody]JiraSettingsUpdateDTO dto)
        {
            IntegrationsService.UpdateJiraSettings(CurrentProject.Id, dto);
            return Ok();
        }

        #endregion
    }
}