﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cog.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private const string CONFIG_APP_ID = "Connectors:Slack:AppId";
        private const string CONFIG_CLIENT_ID = "Connectors:Slack:ClientId";
        private const string CONFIG_CLIENT_SECRET = "Connectors:Slack:ClientSecret";
        private const string AUTH_URL = "https://slack.com/oauth/v2/authorize";
        private const string SCOPE = "commands,incoming-webhook,app_mentions:read,channels:history,channels:join,channels:read,chat:write,chat:write.public,chat:write.customize,groups:history,groups:read,groups:write,im:history,im:read,im:write,mpim:history,mpim:read,mpim:write,usergroups:read,users.profile:read,users:read,users:read.email";

        public SlackConnector(ILogger logger, OrganizationDbContext dataContext, CatalogDbContext catalogDbContext, IConfiguration config) : base(logger, dataContext, catalogDbContext, config)
        {
        }

        public SlackConnector(ILogger logger, IHttpContextAccessor httpContext, OrganizationDbContext dataContext, CatalogDbContext catalogDbContext, IConfiguration config) : base(logger, httpContext, dataContext, catalogDbContext, config)
        {
        }

        public override IntegrationType Type { get; } = IntegrationType.SL;

        #region Public Methods

        public override string GetAuthUrl(OAuthState state)
        {
            return $"{AUTH_URL}?scope={SCOPE}&client_id={Config[CONFIG_CLIENT_ID]}&state={state}&redirect_uri={GetCallbackUrl(state.ToString())}";
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

                var botAccessToken = SlackService.ExchangeCodeForAccessToken(Config[CONFIG_CLIENT_ID], Config[CONFIG_CLIENT_SECRET], code, GetCallbackUrl(state.ToString()))?.Data;

                if (botAccessToken == null)
                    return null;

                var profileIntegration = OrganizationContext.Integrations.Include(x => x.Fields).OrderByDescending(x => x.Created).FirstOrDefault(x => x.ProfileId == state.ProfileId && x.Type == Type);
                var segmentIntegration = OrganizationContext.Integrations.Include(x => x.Fields).OrderByDescending(x => x.Created).FirstOrDefault(x => x.SegmentId == state.SegmentId && x.ProfileId == null && x.Type == Type);
                if (segmentIntegration == null && !state.IsSegmentAuth)
                {
                    throw new CogSecurityException($"profileId: {state.ProfileId} tried to integrate {Type} before segment integration");
                }

                var profileFields = new Dictionary<string, string>
                {
                    [Constants.PROFILE_EXTERNAL_ID] = botAccessToken.AuthedUser.Id
                };

                CreateProfileIntegration(state.ProfileId, state.SegmentId, installationId: null, profileFields, profileIntegration);

                if (state.IsSegmentAuth)
                {
                    var segmentFields = new Dictionary<string, string>
                    {
                        [Constants.ACCESS_TOKEN] = botAccessToken.AccessToken,
                        [Constants.ACCESS_TOKEN_TYPE] = botAccessToken.TokenType,
                        [Constants.SCOPE] = botAccessToken.Scope,
                    };

                    segmentIntegration = CreateSegmentIntegration(state.SegmentId, installationId: null, segmentFields, segmentIntegration);
                }

                OrganizationContext.SaveChanges();
                CatalogContext.SaveChanges();
                return segmentIntegration;
            }
            return null;
        }

        // private void LinkSlackAccountsWithTayraProfileThroughEmailAddress(OrganizationDbContext dbContext, Guid integrationId)
        // {
        //     var segmentIntegration = OrganizationContext.Integrations.Include(x => x.Fields).FirstOrDefault(x => x.Id == integrationId && x.ProfileId == null);
        //     var botToken = segmentIntegration.Fields.FirstOrDefault(x => x.Key == Constants.ACCESS_TOKEN).Value;
        //     var slackUsers = SlackService.GetUsersList(botToken)?.Data;
        //
        //     if (slackUsers == null || !slackUsers.Ok)
        //     {
        //         throw new ApplicationException("could not fetch slack users");
        //     }
        //
        //     var tenantShardingKey = dbContext.CurrentTenantId;
        //     var tenantIdentities = CatalogContext.TenantIdentities
        //         .Where(x => TenantUtilities.ConvertShardingKeyToTenantId(tenantShardingKey) == x.TenantId)
        //         .Select(x => x.IdentityId).ToArray();
        //     
        //     var identityEmails = CatalogContext.IdentityEmails.Where(x => tenantIdentities.Contains(x.IdentityId))
        //         .AsNoTracking().ToArray();
        //     
        //     foreach (var u in slackUsers.Members)
        //     {
        //         if (u.Deleted == false && u.IsBot == false)
        //         {
        //             var identity = identityEmails.FirstOrDefault(x => x.Email.ToLower() == u.Profile.Email.ToLower());
        //             // dbContext.Profiles.FirstOrDefault(x => x.IdentityId == i)
        //             // CreateProfileIntegration(state.ProfileId, state.SegmentId, installationId: null, profileFields,
        //             //     profileIntegration);
        //         }
        //     }
        // }

        public async Task<UsersListResponse> GetUsersList(Guid integrationId)
        {
            var segmentIntegration = await OrganizationContext.Integrations.Include(x => x.Fields).FirstOrDefaultAsync(x => x.Id == integrationId && x.ProfileId == null);
            var botToken = segmentIntegration?.Fields.FirstOrDefault(x => x.Key == Constants.ACCESS_TOKEN)?.Value;
            return SlackService.GetUsersList(botToken)?.Data;
        }

        public override void UpdateAuthentication(string installationId) => throw new NotImplementedException();

        public SlackMessageResponseDto SendSlackMessage(Guid integrationId, SlackMessageRequestDto slackMessageRequestDto)
        {
            var accessToken = ReadAccessToken(integrationId);
            return SlackService.SendSlackMessage(accessToken, slackMessageRequestDto);
        }
        #endregion
    }
}