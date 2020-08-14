using System;
using System.Linq;
using Cog.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.API.Helpers;
using Tayra.Common;
using Tayra.Connectors.Common;
using Tayra.Connectors.GitHub;
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
        public IActionResult Connect([FromRoute] IntegrationType type, [FromQuery] string returnPath, [FromQuery] bool isSegmentAuth)
        {   
            if(string.IsNullOrEmpty(returnPath))
            {
                throw new ApplicationException("You have to provide returnPath");
            }
            
            var connector = ConnectorResolver.Get<IOAuthConnector>(type);
            return Redirect(connector.GetAuthUrl(
                new OAuthState(TenantProvider.GetTenant().Key, CurrentUser.ProfileId, CurrentSegment.Id, isSegmentAuth, returnPath)));
        }

        [HttpGet, Route("settings/atj")]
        public ActionResult<JiraSettingsViewDTO> GetJiraSettings()
        {
            return IntegrationsService.GetJiraSettingsViewDTO("api.tayra.io", CurrentUser.CurrentTenantKey, CurrentSegment.Id);
        }

        [HttpPost, Route("settings/atj")]
        public ActionResult SetJiraSettings([FromBody]JiraSettingsUpdateDTO dto)
        {
            IntegrationsService.UpdateJiraSettingsWithSaveChanges(CurrentSegment.Id, CurrentUser.CurrentTenantKey, dto);
            DbContext.SaveChanges();
            return Ok();
        }

        public class GithubSettingsViewDTO
        {
            public string ExternalConfigurationUrl { get; set; }
            public Repository[] Repositories { get; set; }
            public class Repository
            {
                public string ExternalUrl { get; set; }
                public string Name { get; set; }
                public string NameWithOwner { get; set; }
                public int? TeamId { get; set; }
            }
        }
        
        [HttpGet, Route("settings/gh")]
        public ActionResult<GithubSettingsViewDTO> GetGitHubSettings()
        {
            var integrationId = DbContext.Integrations.Where(x => x.Type == IntegrationType.GH && x.SegmentId == CurrentSegment.Id).Select(x => x.Id).FirstOrDefault();
            var installationId = DbContext.IntegrationFields.Where(x => x.IntegrationId == integrationId && x.Key == GHConstants.GH_INSTALLATION_ID)
                .Select(x => x.Value).FirstOrDefault();

            var repos = DbContext.Repositories.Where(x => x.IntegrationInstallationId == installationId).ToArray();
            
            return Ok(new GithubSettingsViewDTO
            {
                Repositories = repos.Select(x => new GithubSettingsViewDTO.Repository
                {
                    Name = x.Name,
                    NameWithOwner = x.NameWithOwner,
                    ExternalUrl = x.ExternalUrl,
                    TeamId = x.TeamId
                }).ToArray(),
                ExternalConfigurationUrl = $"https://github.com/settings/installations/{installationId}"
            });
        }

        #endregion
    }
}