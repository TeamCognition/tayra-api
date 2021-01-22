using System;
using System.Linq;
using Cog.Core;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using Tayra.API.Helpers;
using Tayra.Common;
using Tayra.Connectors.Common;
using Tayra.Connectors.GitHub;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.SyncServices;

namespace Tayra.API.Controllers
{
    public class IntegrationsController : BaseDataController
    {
        #region Constructor

        public IntegrationsController(IServiceProvider serviceProvider, IConnectorResolver connectorResolver, OrganizationDbContext context) : base(serviceProvider, context)
        {
            ConnectorResolver = connectorResolver;
            DbContext = context;
        }

        #endregion

        OrganizationDbContext DbContext { get; set; }

        public IConnectorResolver ConnectorResolver { get; }

        #region Public Methods

        [HttpGet, Route("connect/{type?}")]
        public IActionResult Connect([FromRoute] IntegrationType type, [FromQuery] string returnPath, [FromQuery] bool isSegmentAuth)
        {
            if (string.IsNullOrEmpty(returnPath))
            {
                throw new ApplicationException("You have to provide returnPath");
            }

            var tenantInfo = HttpContext.GetMultiTenantContext<Tenant>()?.TenantInfo;
            
            var connector = ConnectorResolver.Get<IOAuthConnector>(type);
            return Redirect(connector.GetAuthUrl(
                new OAuthState(tenantInfo.Identifier, CurrentUser.ProfileId, CurrentSegment.Id, isSegmentAuth, returnPath)));
        }

        [HttpGet, Route("settings/atj")]
        public ActionResult<JiraSettingsViewDTO> GetJiraSettings([FromServices]IConfiguration config)
        {
            return IntegrationsService.GetJiraSettingsViewDTO("api.tayra.io", CurrentUser.CurrentTenantIdentifier, CurrentSegment.Id, config);
        }

        [HttpPost, Route("settings/atj")]
        public ActionResult SetJiraSettings([FromBody] JiraSettingsUpdateDTO dto, [FromServices]IConfiguration config)
        {
            IntegrationsService.UpdateJiraSettingsWithSaveChanges(CurrentSegment.Id, CurrentUser.CurrentTenantIdentifier, dto, config);
            // SyncIssuesLoader.PullIssuesNew(DbContext, DateTime.UtcNow, TasksService, ProfilesService,
            //     JObject.FromObject(new { tenantKey = CurrentUser.CurrentTenantKey, @params = new { jiraProjectId = dto.ActiveProjects.FirstOrDefault().ProjectId }}));
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
                public Guid? TeamId { get; set; }
            }
        }

        [HttpGet, Route("settings/gh")]
        public ActionResult<GithubSettingsViewDTO> GetGitHubSettings()
        {
            var integrationId = DbContext.Integrations.Where(x => x.Type == IntegrationType.GH && x.SegmentId == CurrentSegment.Id && x.ProfileId == null).Select(x => x.Id).FirstOrDefault();
            var fields = DbContext.IntegrationFields.Where(x => x.IntegrationId == integrationId).ToArray();
            var installationId = fields.FirstOrDefault(x => x.Key == GHConstants.GH_INSTALLATION_ID)?.Value;
            var targetType = fields.FirstOrDefault(x => x.Key == GHConstants.GH_INSTALLATION_TARGET_TYPE)?.Value;
            var targetName = fields.FirstOrDefault(x => x.Key == GHConstants.GH_INSTALLATION_TARGET_NAME)?.Value;
            var externalConfigUrl = targetType == "Organization"
                ? $"https://github.com/organizations/{targetName}/settings/installations/{installationId}"
                : $"https://github.com/settings/installations/{installationId}";

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
                ExternalConfigurationUrl = externalConfigUrl
            });
        }

        #endregion
    }
}