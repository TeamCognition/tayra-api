using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tayra.Common;
using Tayra.Connectors.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.Slack
{
    public class SlackConnector : BaseOAuthConnector
    {
        private const string SEGMENT_AUTH_URL = "https://slack.com/oauth/v2/authorize";
        private const string PROFILE_AUTH_URL = "https://slack.com/oauth/v2/authorize";
        
        private const string SLAPP_ID = "A013UGRR7FW";
        
        private const string SCOPE = "incoming-webhook,commands";
        public SlackConnector(ILogger logger, OrganizationDbContext dataContext, CatalogDbContext catalogDbContext) : base(logger, dataContext, catalogDbContext)
        {
        }

        public SlackConnector(ILogger logger, IHttpContextAccessor httpContext, ITenantProvider tenantProvider, OrganizationDbContext dataContext, CatalogDbContext catalogDbContext) : base(logger, httpContext, tenantProvider, dataContext, catalogDbContext)
        {
        }

        public override IntegrationType Type { get; } = IntegrationType.SL;

        #region Public Methods

        public override string GetAuthUrl(OAuthState state)
        {
            return $"{SEGMENT_AUTH_URL}?scope={SCOPE}&client_id={SlackService.CLIENT_ID}&state={state}&redirect_uri={GetCallbackUrl(state.ToString())}";
        }

        public override Integration Authenticate(OAuthState state)
        {
            if (HttpContext?.Request != null)
            {
                var code = HttpContext.Request.Query["code"];
                if (string.IsNullOrWhiteSpace(code))
                {
                    var errorDescription = HttpContext.Request.Query["error"];
                    throw new ApplicationException(errorDescription);
                }

                var userTokenData = SlackService.GetUserAccessToken(code, GetCallbackUrl(state.ToString()))?.Data;
                //var loggedInUser = SlackService.GetLoggedInUser(userTokenData.TokenType, userTokenData.AccessToken);
                
                // var profileIntegration = OrganizationContext.Integrations.Include(x => x.Fields).LastOrDefault(x => x.ProfileId == state.ProfileId && x.Type == Type);
                // var segmentIntegration = OrganizationContext.Integrations.Include(x => x.Fields).LastOrDefault(x => x.SegmentId == state.SegmentId && x.ProfileId == null && x.Type == Type);
                // if (segmentIntegration == null && !state.IsSegmentAuth)
                // {
                //     throw new CogSecurityException($"profileId: {state.ProfileId} tried to integrate {Type} before segment integration");
                // }
                //
                // if (loggedInUser != null)
                // {
                //     var profileFields = new Dictionary<string, string>
                //     {
                //         [Constants.PROFILE_EXTERNAL_ID] = loggedInUser.Login
                //     };
                //
                //     CreateProfileIntegration(state.ProfileId, state.SegmentId, installationId: null, profileFields, profileIntegration);
                // }
                //
                // if (state.IsSegmentAuth && userTokenData != null)
                // {
                //     var installations = SlackService.GetUserInstallations(userTokenData.TokenType, userTokenData.AccessToken);
                //     var installation = Utils.FindTayraAppInstallation(installations?.Data.Installations, GHAPP_ID);
                //     var installationId = installation.Id;
                //     var installationToken = SlackService.GetInstallationAccessToken(installationId, GHAPP_ID, GHAPP_RSA_KEY)?.Data.AccessToken;;
                //
                //     var repositories = SlackService.GetInstallationRepositories(installationToken)?.Data?.Repositories;
                //
                //     AddOrUpdateRepositoriesByIntegration(installationId, repositories);
                //     AddOrUpdateWebhooks(installationId, installationToken, repositories);
                //
                //     var targetName = installation.TargetType == "Organization"
                //         ? Utils.GetInstallationOrganizationName(userTokenData.AccessToken, installation.TargetId)
                //         : loggedInUser?.Login;
                //     
                //     var segmentFields = new Dictionary<string, string>
                //     {
                //         [Constants.ACCESS_TOKEN] = userTokenData.AccessToken,
                //         [Constants.ACCESS_TOKEN_TYPE] = userTokenData.TokenType,
                //         [Constants.SCOPE] = userTokenData.Scope,
                //         [GHConstants.GH_INSTALLATION_ID] = installationId,
                //         [GHConstants.GH_INSTALLATION_TARGET_TYPE] = installation.TargetType,
                //         [GHConstants.GH_INSTALLATION_TARGET_NAME] = targetName
                //     };
                //
                //     segmentIntegration = CreateSegmentIntegration(state.SegmentId, installationId, segmentFields, segmentIntegration);
                // }
                //
                // var unlinkedGitCommits = OrganizationContext.GitCommits.Where(x => x.AuthorProfileId == null && x.AuthorExternalId == loggedInUser.Id);
                // foreach (var commit in unlinkedGitCommits)
                // {
                //     commit.AuthorProfileId = state.ProfileId;
                // }
                //
                // OrganizationContext.SaveChanges();
                // CatalogContext.SaveChanges();
                return null;
            }
            return null;
        }

        public override void UpdateAuthentication(string installationId)
        {
            // var installationToken = SlackService.GetInstallationAccessToken(installationId, GHAPP_ID, GHAPP_RSA_KEY)?.Data.AccessToken;;
            //
            // var repositories = SlackService.GetInstallationRepositories(installationToken)?.Data?.Repositories;
            //
            // AddOrUpdateRepositoriesByIntegration(installationId, repositories);
            // AddOrUpdateWebhooks(installationId, installationToken, repositories);
            //
            // OrganizationContext.SaveChanges();
        }
        
        #endregion
    }
}