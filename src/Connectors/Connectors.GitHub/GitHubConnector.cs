﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tayra.Common;
using Tayra.Connectors.Common;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.GitHub
{
    public class GitHubConnector : BaseOAuthConnector
    {
        private const string AUTH_URL = "https://github.com/apps/tayra12/installations/new";

        public GitHubConnector(ILogger logger, OrganizationDbContext dataContext) : base(logger, dataContext)
        {
        }

        public GitHubConnector(ILogger logger, IHttpContextAccessor httpContext, ITenantProvider tenantProvider, OrganizationDbContext dataContext) : base(logger, httpContext, tenantProvider, dataContext)
        {
        }

        public override IntegrationType Type { get; } = IntegrationType.GH;

        #region Public Methods

        public override string GetAuthUrl(string userState)
        {
            return $"{AUTH_URL}?state={userState}";
        }

        public override Integration Authenticate(int profileId, ProfileRoles profileRole, int segmentId, string userState)
        {
            if (HttpContext?.Request != null)
            {
                var code = HttpContext.Request.Query["code"];
                if (string.IsNullOrWhiteSpace(code))
                {
                    var errorDescription = HttpContext.Request.Query["error"];
                    throw new ApplicationException(errorDescription);
                }
                
                var tokenData = GitHubService.GetAccessToken(code, GetCallbackUrl(userState))?.Data;
                var loggedInUser = GitHubService.GetLoggedInUser(tokenData.TokenType, tokenData.AccessToken);

                var profileIntegration = OrganizationContext.Integrations.Include(x => x.Fields).LastOrDefault(x => x.ProfileId == profileId && x.Type == Type);
                var segmentIntegration = OrganizationContext.Integrations.Include(x => x.Fields).LastOrDefault(x => x.SegmentId == segmentId && x.ProfileId == null && x.Type == Type);
                if (segmentIntegration == null && profileRole == ProfileRoles.Member)
                {
                    throw new CogSecurityException($"profileId: {profileId} tried to integrate {Type} before segment integration");
                }

                if (loggedInUser != null)
                {
                    var profileFields = new Dictionary<string, string>
                    {
                        [Constants.PROFILE_EXTERNAL_ID] = loggedInUser.Login
                    };
                
                    CreateProfileIntegration(profileId, segmentId, profileFields, profileIntegration);
                }
                
                if (profileRole != ProfileRoles.Member && tokenData != null)
                {
                    var segmentFields = new Dictionary<string, string>
                    {
                        [Constants.ACCESS_TOKEN] = tokenData.AccessToken,
                        [Constants.ACCESS_TOKEN_TYPE] = tokenData.TokenType,
                        [Constants.SCOPE] = tokenData.Scope,
                    };
                
                    segmentIntegration = CreateSegmentIntegration(segmentId, segmentFields, segmentIntegration);
                }

                var unlinkedGitCommits = OrganizationContext.GitCommits.Where(x => x.AuthorProfileId == null && x.AuthorExternalId == loggedInUser.Id);
                foreach (var commit in unlinkedGitCommits)
                {
                    commit.AuthorProfileId = profileId;
                }

                OrganizationContext.SaveChanges();
                return segmentIntegration;
            }
            return null;
        }
        
        #endregion
    }
}