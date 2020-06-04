using System;
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
        private const string AUTH_URL = "https://github.com/apps/tayra12/installations/new/permissions";

        public GitHubConnector(ILogger logger, OrganizationDbContext dataContext) : base(logger, dataContext)
        {
        }

        public GitHubConnector(ILogger logger, IHttpContextAccessor httpContext, ITenantProvider tenantProvider, OrganizationDbContext dataContext) : base(logger, httpContext, tenantProvider, dataContext)
        {
        }

        public override IntegrationType Type { get; } = IntegrationType.ATJ;

        #region Public Methods

        public override string GetAuthUrl(string userState)
        {
            return $"{AUTH_URL}&state={GetCallbackUrl(userState)}";
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
                var loggedInUser = GitHubService.GetLoggedInUser(tokenData.TokenType, tokenData.AccessToken)?.Data;

                var profileIntegration = OrganizationContext.Integrations.Include(x => x.Fields).LastOrDefault(x => x.ProfileId == profileId && x.Type == Type);
                var segmentIntegration = OrganizationContext.Integrations.Include(x => x.Fields).LastOrDefault(x => x.SegmentId == segmentId && x.ProfileId == null && x.Type == Type);
                if (segmentIntegration == null && profileRole == ProfileRoles.Member)
                {
                    throw new CogSecurityException($"profileId: {profileId} tried to integrate {Type} before segment integration");
                }

                //if (loggedInUser != null)
                //{
                //    var profileFields = new Dictionary<string, string>
                //    {
                //        [Constants.PROFILE_EXTERNAL_ID] = loggedInUser.AccountId
                //    };

                //    CreateProfileIntegration(profileId, segmentId, profileFields, profileIntegration);
                //}

                //if (profileRole != ProfileRoles.Member && tokenData != null && accResData != null)
                //{
                //    var segmentFields = new Dictionary<string, string>
                //    {
                //        [Constants.ACCESS_TOKEN] = tokenData.AccessToken,
                //        [Constants.ACCESS_TOKEN_TYPE] = tokenData.TokenType,
                //        [Constants.ACCESS_EXPIRATION] = tokenData.ExpirationDate,
                //        [Constants.REFRESH_TOKEN] = tokenData.RefreshToken,
                //        [Constants.SCOPE] = tokenData.Scope,
                //    };

                //    segmentIntegration = CreateSegmentIntegration(segmentId, segmentFields, segmentIntegration);
                //}

                //var unlinkedTasks = OrganizationContext.Tasks.Where(x => x.AssigneeProfileId == null && x.AssigneeExternalId == loggedInUser.AccountId);
                //foreach (var ut in unlinkedTasks)
                //{
                //    ut.AssigneeProfileId = profileId;
                //}

                //OrganizationContext.SaveChanges();
                //return segmentIntegration;
            }
            return null;
        }

        public override Integration RefreshToken(int integrationId)
        {
            var account = OrganizationContext
                .Integrations
                .Include(a => a.Fields)
                .FirstOrDefault(a => a.Id == integrationId);

            if (account == null)
            {
                throw new ApplicationException("The account does not exist.");
            }

            var refreshCode = account.Fields.FirstOrDefault(f => f.Key == Constants.REFRESH_TOKEN);
            if (refreshCode == null)
            {
                throw new ApplicationException("Unable to refresh this account because it does not have a refresh token.");
            }

            var response = GitHubService.RefreshAccessToken(refreshCode.Value);
            if (response?.Data != null)
            {
                void Update(string key, string value)
                {
                    var field = account.Fields.FirstOrDefault(f => f.Key == key);
                    if (field != null)
                    {
                        field.Value = value;
                    }
                    else
                    {
                        account.Fields.Add(new IntegrationField { Key = key, Value = value });
                    }
                }

                Update(Constants.ACCESS_TOKEN, response.Data.AccessToken);
                Update(Constants.ACCESS_TOKEN_TYPE, response.Data.TokenType);
                Update(Constants.ACCESS_EXPIRATION, response.Data.ExpirationDate);
                Update(Constants.SCOPE, response.Data.Scope);


                OrganizationContext.SaveChanges();
            }
            else
            {
                throw new ApplicationException(response?.ErrorMessage ?? "Something went wrong");
            }

            return account;
        }
        #endregion
    }
}