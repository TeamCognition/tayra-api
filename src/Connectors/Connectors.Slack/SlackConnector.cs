using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tayra.Common;
using Tayra.Connectors.Common;
using Tayra.Connectors.Slack.DTOs;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.Slack
{
    public class SlackConnector : BaseOAuthConnector
    {
        private const string AUTH_URL = "https://slack.com/oauth/v2/authorize";
        private const string SLAPP_ID = "A013UGRR7FW";
        private const string SCOPE = "commands,incoming-webhook,app_mentions:read,channels:history,channels:join,channels:read,chat:write,chat:write.public,chat:write.customize,groups:history,groups:read,groups:write,im:history,im:read,im:write,mpim:history,mpim:read,mpim:write,usergroups:read,users.profile:read,users:read,users:read.email";

        public SlackConnector(ILogger logger, OrganizationDbContext dataContext, CatalogDbContext catalogDbContext) : base(logger, dataContext, catalogDbContext)
        {
        }

        public SlackConnector(ILogger logger, IHttpContextAccessor httpContext, OrganizationDbContext dataContext, CatalogDbContext catalogDbContext) : base(logger, httpContext, dataContext, catalogDbContext)
        {
        }

        public override IntegrationType Type { get; } = IntegrationType.SL;

        #region Public Methods

        public override string GetAuthUrl(OAuthState state)
        {
            return $"{AUTH_URL}?scope={SCOPE}&client_id={SlackService.CLIENT_ID}&state={state}&redirect_uri={GetCallbackUrl(state.ToString())}";
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

                var accessToken = SlackService.ExchangeCodeForAccessToken(code, GetCallbackUrl(state.ToString()))?.Data;

                if (accessToken == null)
                    return null;

                var profileIntegration = OrganizationContext.Integrations.Include(x => x.Fields).LastOrDefault(x => x.ProfileId == state.ProfileId && x.Type == Type);
                var segmentIntegration = OrganizationContext.Integrations.Include(x => x.Fields).LastOrDefault(x => x.SegmentId == state.SegmentId && x.ProfileId == null && x.Type == Type);
                if (segmentIntegration == null && !state.IsSegmentAuth)
                {
                    throw new CogSecurityException($"profileId: {state.ProfileId} tried to integrate {Type} before segment integration");
                }

                var profileFields = new Dictionary<string, string>
                {
                    [Constants.PROFILE_EXTERNAL_ID] = accessToken.AuthedUser.Id
                };

                CreateProfileIntegration(state.ProfileId, state.SegmentId, installationId: null, profileFields, profileIntegration);

                if (state.IsSegmentAuth)
                {
                    var segmentFields = new Dictionary<string, string>
                    {
                        [Constants.ACCESS_TOKEN] = accessToken.AccessToken,
                        [Constants.ACCESS_TOKEN_TYPE] = accessToken.TokenType,
                        [Constants.SCOPE] = accessToken.Scope,
                    };

                    segmentIntegration = CreateSegmentIntegration(state.SegmentId, installationId: null, segmentFields, segmentIntegration);
                }

                OrganizationContext.SaveChanges();
                CatalogContext.SaveChanges();
                return segmentIntegration;
            }
            return null;
        }

        private void LinkSlackAccountsWithTayraProfileThroughEmailAddress(OrganizationDbContext dbContext, Guid integrationId)
        {
            var segmentIntegration = OrganizationContext.Integrations.Include(x => x.Fields).FirstOrDefault(x => x.Id == integrationId && x.ProfileId == null);
            var botToken = segmentIntegration.Fields.FirstOrDefault(x => x.Key == Constants.ACCESS_TOKEN).Value;
            var slackUsers = SlackService.GetUsersList(botToken)?.Data;

            if (slackUsers == null || !slackUsers.Ok)
            {
                throw new ApplicationException("could not fetch slack users");
            }

            // var tenantShardingKey = dbContext.CurrentTenantId;
            // var tenantIdentities = CatalogContext.TenantIdentities
            //     .Where(x => TenantUtilities.ConvertShardingKeyToTenantId(tenantShardingKey) == x.TenantId)
            //     .Select(x => x.IdentityId).ToArray();
            //
            // var identityEmails = CatalogContext.IdentityEmails.Where(x => tenantIdentities.Contains(x.IdentityId))
            //     .AsNoTracking().ToArray();
            //
            // foreach (var u in slackUsers.Members)
            // {
            //     if (u.Deleted == false && u.IsBot == false)
            //     {
            //         var identity = identityEmails.FirstOrDefault(x => x.Email.ToLower() == u.Profile.Email.ToLower());
            //         // dbContext.Profiles.FirstOrDefault(x => x.IdentityId == i)
            //         // CreateProfileIntegration(state.ProfileId, state.SegmentId, installationId: null, profileFields,
            //         //     profileIntegration);
            //     }
            // }
        }

        public override void UpdateAuthentication(string installationId) => throw new NotImplementedException();

        public SlackMessageResponseDto SendSlackMessage(Guid integrationId, SlackMessageRequestDto slackMessageRequestDto)
        {
            var accessToken = ReadAccessToken(integrationId);
            return SlackService.SendSlackMessage(accessToken, slackMessageRequestDto).Data;
        }
        #endregion
    }
}