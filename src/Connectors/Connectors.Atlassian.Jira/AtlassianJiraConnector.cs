using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tayra.Common;
using Tayra.Connectors.Common;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class AtlassianJiraConnector : BaseOAuthConnector
    {
        private const string AUTH_URL = "https://auth.atlassian.com/authorize";
        private const string AUDIENCE = "api.atlassian.com";
        private const string SCOPE = "read%3Ajira-user%20read%3Ajira-work%20offline_access";

        public AtlassianJiraConnector(ILogger logger, OrganizationDbContext dataContext) : base(logger, dataContext)
        {
        }

        public AtlassianJiraConnector(ILogger logger, IHttpContextAccessor httpContext, ITenantProvider tenantProvider, OrganizationDbContext dataContext) : base(logger, httpContext, tenantProvider, dataContext)
        {
        }

        public override IntegrationType Type { get; } = IntegrationType.ATJ;

        #region Public Methods

        public override string GetAuthUrl(string userState)
        {
            return $"{AUTH_URL}?audience={AUDIENCE}&client_id={AtlassianJiraService.APP_ID}&state={userState}&scope={SCOPE}&redirect_uri={GetCallbackUrl(userState)}&response_type=code&prompt=consent";
        }

        public override Integration Authenticate(int profileId, ProfileRoles profileRole, int segmentId, string userState)
        {
            if (HttpContext?.Request != null)
            {
                var authorizationCode = HttpContext.Request.Query["code"];
                if (string.IsNullOrWhiteSpace(authorizationCode))
                {
                    var errorDescription = HttpContext.Request.Query["error"];
                    throw new ApplicationException(errorDescription);
                }

                var tokenData = AtlassianJiraService.GetAccessToken(authorizationCode, GetCallbackUrl(userState))?.Data;
                var accResData = AtlassianJiraService.GetAccessibleResources(tokenData.TokenType, tokenData.AccessToken)?.Data?.FirstOrDefault();
                var loggedInUser = AtlassianJiraService.GetLoggedInUser(accResData.CloudId, tokenData.TokenType, tokenData.AccessToken)?.Data;

                var profileIntegration = OrganizationContext.Integrations.Include(x => x.Fields).LastOrDefault(x => x.ProfileId == profileId && x.Type == Type);
                var segmentIntegration = OrganizationContext.Integrations.Include(x => x.Fields).LastOrDefault(x => x.ProfileId == null && x.Type == Type);
                if (segmentIntegration == null && profileRole == ProfileRoles.Member)
                {
                    throw new FirdawsSecurityException($"profileId: {profileId} tried to integrate {Type} before segment integration");
                }

                if (loggedInUser != null)
                {
                    var profileFields = new Dictionary<string, string>
                    {
                        [Constants.PROFILE_EXTERNAL_ID] = loggedInUser.AccountId
                    };

                    CreateProfileIntegration(profileId, segmentId, profileFields, profileIntegration);
                }

                if (profileRole != ProfileRoles.Member && tokenData != null && accResData != null)
                {
                    var segmentFields = new Dictionary<string, string>
                    {
                        [Constants.ACCESS_TOKEN] = tokenData.AccessToken,
                        [Constants.ACCESS_TOKEN_TYPE] = tokenData.TokenType,
                        [Constants.ACCESS_EXPIRATION] = tokenData.ExpirationDate,
                        [Constants.REFRESH_TOKEN] = tokenData.RefreshToken,
                        [Constants.SCOPE] = tokenData.Scope,
                        [ATConstants.AT_CLOUD_ID] = accResData.CloudId,
                        [ATConstants.AT_SITE_NAME] = accResData.Name
                    };

                    segmentIntegration = CreateSegmentIntegration(segmentId, segmentFields, segmentIntegration);
                }

                OrganizationContext.SaveChanges();
                return segmentIntegration;
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

            var response = AtlassianJiraService.RefreshAccessToken(refreshCode.Value);
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

        public ICollection<JiraProject> GetProjects(int integrationId)
        {
            var integration = RefreshToken(integrationId);
            if (integration == null)
            {
                throw new ApplicationException("Integration does not exist");
            }

            var accessToken = ReadAccessToken(integrationId);
            var accessTokenType = ReadAccessTokenType(integrationId);
            var cloudId = ReadCloudId(integrationId);

            return AtlassianJiraService.GetProjects(cloudId, accessTokenType, accessToken).Data.Values;
        }

        public ICollection<JiraStatus> GetProjectStatuses(int integrationId, string jiraProjectId, string jiraIssueTypeId = "")
        {
            var integration = RefreshToken(integrationId);
            if (integration == null)
            {
                throw new ApplicationException("Integration does not exist");
            }

            var accessToken = ReadAccessToken(integrationId);
            var accessTokenType = ReadAccessTokenType(integrationId);
            var cloudId = ReadCloudId(integrationId);

            var issues = AtlassianJiraService.GetProjectStatuses(cloudId, accessTokenType, accessToken, jiraProjectId).Data;
            if (string.IsNullOrEmpty(jiraIssueTypeId))
            {
                return issues.SelectMany(x => x.Statuses).Distinct().ToList();
            }
            else
            {
                return issues.Where(x => x.Id == jiraIssueTypeId).SelectMany(x => x.Statuses).ToList();
            }
        }

        public JiraIssue GetIssue(int integrationId, string issueIdOrKey, string expand = "")
        {
            var integration = RefreshToken(integrationId);
            if (integration == null)
            {
                throw new ApplicationException("Integration does not exist");
            }

            var accessToken = ReadAccessToken(integrationId);
            var accessTokenType = ReadAccessTokenType(integrationId);
            var cloudId = ReadCloudId(integrationId);

            return AtlassianJiraService.GetIssue(cloudId, accessTokenType, accessToken, issueIdOrKey, expand).Data;
        }

        public ICollection<IssueChangelog> GetIssueChangelog(int integrationId, string issueIdOrKey, string fieldFilter = "")
        {
            var integration = RefreshToken(integrationId);
            if (integration == null)
            {
                throw new ApplicationException("Integration does not exist");
            }

            var accessToken = ReadAccessToken(integrationId);
            var accessTokenType = ReadAccessTokenType(integrationId);
            var cloudId = ReadCloudId(integrationId);

            var jiraChangelogs = AtlassianJiraService.GetIssueChangelog(cloudId, accessTokenType, accessToken, issueIdOrKey).Data.Values;

            var changelogs = new List<IssueChangelog>();
            foreach(var cl in jiraChangelogs)
            {
                cl.Items.Where(x => string.IsNullOrEmpty(fieldFilter) || x.Field == fieldFilter).ToList().ForEach(x => 
                changelogs.Add(new IssueChangelog
                {
                    Created = cl.Created,
                    Field = x.Field,
                    From = x.From,
                    To = x.To,
                    Author = new IssueChangelog.AuthorMeta
                    {
                        AccountId = cl.Author.AccountId,
                        DisplayName = cl.Author.DisplayName,
                        EmailAddress = cl.Author.EmailAddress
                    }
                }));
            }

            return changelogs;
        }

        public List<JiraUser> GetUsers(int integrationId)
        {
            var integration = RefreshToken(integrationId);
            if (integration == null)
            {
                throw new ApplicationException("Integration does not exist");
            }

            var accessToken = ReadAccessToken(integrationId);
            var accessTokenType = ReadAccessTokenType(integrationId);
            var cloudId = ReadCloudId(integrationId);

            var jiraUsers = AtlassianJiraService.GetUsers(cloudId, accessTokenType, accessToken).Data;

            jiraUsers.RemoveAll(x => x.AccountType != "atlassian");
           
            return jiraUsers;
        }

        public List<JiraIssueType> GetIssueTypes(int integrationId)
        {
            var integration = RefreshToken(integrationId);
            if (integration == null)
            {
                throw new ApplicationException("Integration does not exist");
            }

            var accessToken = ReadAccessToken(integrationId);
            var accessTokenType = ReadAccessTokenType(integrationId);
            var cloudId = ReadCloudId(integrationId);

            var issueTypes = AtlassianJiraService.GetIssueTypes(cloudId, accessTokenType, accessToken).Data;

            return issueTypes;
        }

        #endregion

        #region Private Methods

        private string ReadCloudId(int integrationId)
        {
            return ReadField(integrationId, ATConstants.AT_CLOUD_ID, "Unknown cloud id for integration " + integrationId);
        }

        #endregion

    }
}