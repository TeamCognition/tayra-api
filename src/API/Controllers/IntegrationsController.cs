using System;
using Firdaws.Core;
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

        public IntegrationsController(IServiceProvider serviceProvider, IConnectorResolver connectorResolver, ITenantProvider tenantProvider, OrganizationDbContext context) : base(serviceProvider, context)
        {
            ConnectorResolver = connectorResolver;
            DbContext = context;
            TenantProvider = tenantProvider;
        }

        #endregion

        public ITenantProvider TenantProvider { get; set; }
        OrganizationDbContext DbContext { get; set; }

        public IConnectorResolver ConnectorResolver { get; }

        #region Public Methods

        [HttpGet, Route("connect/{type?}")]
        public IActionResult Connect([FromRoute] IntegrationType type, [FromQuery] string returnPath)
        {   
            if(string.IsNullOrEmpty(returnPath))
            {
                throw new ApplicationException("You have to provide returnPath");
            }
            
            var connector = ConnectorResolver.Get<IOAuthConnector>(type);
            return Redirect(connector.GetAuthUrl(Cipher.Encrypt(string.Join('|', TenantProvider.GetTenant().Host, CurrentUser.ProfileId, CurrentUser.Role, CurrentSegment.Id, returnPath))));
        }

        [HttpGet, Route("settings/atj")]
        public ActionResult<JiraSettingsViewDTO> GetJiraSettings()
        {
            return IntegrationsService.GetJiraSettingsViewDTO(CurrentSegment.Id);
        }

        [HttpPost, Route("settings/atj")]
        public ActionResult SetJiraSettings([FromBody]JiraSettingsUpdateDTO dto)
        {
            IntegrationsService.UpdateJiraSettings(CurrentSegment.Id, dto);
            DbContext.SaveChanges();
            return Ok();
        }

        #endregion
    }
}