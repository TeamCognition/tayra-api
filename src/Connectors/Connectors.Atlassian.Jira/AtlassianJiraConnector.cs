using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tayra.Common;
using Tayra.Connectors.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class AtlassianJiraConnector : BaseOAuthConnector
    {
        private const string CONFIG_APP_ID = "Connectors:Atlassian:Jira:ClientId";
        private const string CONFIG_APP_SECRET = "Connectors:Atlassian:Jira:ClientSecret";
        private const string AUTH_URL = "https://auth.atlassian.com/authorize";
        private const string AUDIENCE = "api.atlassian.com";
        private const string SCOPE = "read%3Ajira-user%20read%3Ajira-work%20offline_access";

        public AtlassianJiraConnector(ILogger logger, OrganizationDbContext dataContext, CatalogDbContext catalogDbContext, IConfiguration config) : base(logger, dataContext, catalogDbContext, config)
        {
        }

        public AtlassianJiraConnector(ILogger logger, IHttpContextAccessor httpContext, OrganizationDbContext dataContext, CatalogDbContext catalogDbContext, IConfiguration config) : base(logger, httpContext, dataContext, catalogDbContext, config)
        {
        }

        public override IntegrationType Type { get; } = IntegrationType.ATJ;

        #region Public Methods

        public override string GetAuthUrl(OAuthState state)
        {
            return $"{AUTH_URL}?audience={AUDIENCE}&client_id={Config[CONFIG_APP_ID]}&state={state}&scope={SCOPE}&redirect_uri={GetCallbackUrl(state.ToString())}&response_type=code&prompt=consent";
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

                var tokenData = AtlassianJiraService.GetAccessToken(Config[CONFIG_APP_ID], Config[CONFIG_APP_SECRET], code, GetCallbackUrl(state.ToString()))?.Data;
                var accResData = AtlassianJiraService.GetAccessibleResources(tokenData.TokenType, tokenData.AccessToken)?.Data?.FirstOrDefault();
                var loggedInUser = AtlassianJiraService.GetLoggedInUser(accResData.CloudId, tokenData.TokenType, tokenData.AccessToken)?.Data;

                var profileIntegration = OrganizationContext.Integrations.Include(x => x.Fields).LastOrDefault(x => x.ProfileId == state.ProfileId && x.Type == Type);
                var segmentIntegration = OrganizationContext.Integrations.Include(x => x.Fields).LastOrDefault(x => x.SegmentId == state.SegmentId && x.ProfileId == null && x.Type == Type);
                if (segmentIntegration == null && !state.IsSegmentAuth)
                {
                    throw new CogSecurityException($"profileId: {state.ProfileId} tried to integrate {Type} before segment integration");
                }

                if (loggedInUser != null)
                {
                    var profileFields = new Dictionary<string, string>
                    {
                        [Constants.PROFILE_EXTERNAL_ID] = loggedInUser.AccountId
                    };

                    CreateProfileIntegration(state.ProfileId, state.SegmentId, installationId: null, profileFields, profileIntegration);
                }

                if (state.IsSegmentAuth && tokenData != null && accResData != null)
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

                    segmentIntegration = CreateSegmentIntegration(state.SegmentId, installationId: null, segmentFields, segmentIntegration);
                }

                var unlinkedTasks = OrganizationContext.Tasks.Where(x => (x.AssigneeProfileId == null || x.TeamId == null) && x.AssigneeExternalId == loggedInUser.AccountId);
                var profileAssignment =
                    OrganizationContext.ProfileAssignments.FirstOrDefault(x => x.ProfileId == state.ProfileId);
                foreach (var ut in unlinkedTasks)
                {
                    ut.AssigneeProfileId = state.ProfileId;
                    ut.TeamId = profileAssignment?.TeamId;
                    ut.SegmentId = profileAssignment?.SegmentId;
                }

                OrganizationContext.SaveChanges();
                return segmentIntegration;
            }
            return null;
        }

        public override void UpdateAuthentication(string installationId) => throw new NotImplementedException();
        public override Integration RefreshToken(Guid integrationId)
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

            var response = AtlassianJiraService.RefreshAccessToken(Config[CONFIG_APP_ID], Config[CONFIG_APP_SECRET], refreshCode.Value);
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

        public ICollection<JiraProject> GetProjects(Guid integrationId)
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

        public ICollection<JiraIssueType> GetIssueTypesWithStatuses(Guid integrationId, string jiraProjectId)
        {
            var integration = RefreshToken(integrationId);
            if (integration == null)
            {
                throw new ApplicationException("Integration does not exist");
            }

            var accessToken = ReadAccessToken(integrationId);
            var accessTokenType = ReadAccessTokenType(integrationId);
            var cloudId = ReadCloudId(integrationId);

            return AtlassianJiraService.GetProjectStatuses(cloudId, accessTokenType, accessToken, jiraProjectId).Data;
        }

        public ICollection<JiraStatus> GetIssueStatuses(Guid integrationId, string jiraProjectId, string jiraIssueTypeId = "")
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
                return issues.SelectMany(x => x.Statuses).Distinct().ToArray();
            }
            else
            {
                return issues.Where(x => x.Id == jiraIssueTypeId).SelectMany(x => x.Statuses).ToArray();
            }
        }

        public JiraIssue GetIssue(Guid integrationId, string issueIdOrKey, string expand = "")
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

        public List<TaskChangelog> GetIssueChangelog(Guid integrationId, string issueIdOrKey, string fieldFilter = "")
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

            var changelogs = new List<TaskChangelog>();
            foreach (var cl in jiraChangelogs)
            {
                cl.Items.Where(x => string.IsNullOrEmpty(fieldFilter) || x.Field == fieldFilter).ToList().ForEach(x =>
                changelogs.Add(new TaskChangelog
                {
                    Created = cl.Created,
                    Field = x.Field,
                    From = x.From,
                    To = x.To,
                    Author = new TaskChangelog.AuthorMeta
                    {
                        AccountId = cl.Author.AccountId,
                        DisplayName = cl.Author.DisplayName,
                        EmailAddress = cl.Author.EmailAddress
                    }
                }));
            }

            return changelogs;
        }

        public List<JiraUser> GetUsers(Guid integrationId)
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

        public List<JiraIssueType> GetIssueTypes(Guid integrationId)
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

        public List<JiraIssue> GetBulkIssuesWithChangelog(Guid integrationId, string fieldFilter = "status", params string[] jiraProjects)
        {
            var integration = RefreshToken(integrationId);
            if (integration == null)
            {
                throw new ApplicationException("Integration does not exist");
            }

            var accessToken = ReadAccessToken(integrationId);
            var accessTokenType = ReadAccessTokenType(integrationId);
            var cloudId = ReadCloudId(integrationId);

            var issueSearch = AtlassianJiraService.GetBulkIssuesWithChangelog(cloudId, accessTokenType, accessToken, 90, jiraProjects).Data;
            foreach (var issue in issueSearch.Issues)
            {
                var changelogs = new List<TaskChangelog>();

                foreach (var cl in issue.JiraSearchChangelog.Histories.OrderBy(x => x.Created))
                {
                    cl.Items.Where(x => string.IsNullOrEmpty(fieldFilter) || x.Field == fieldFilter).ToList().ForEach(x =>
                    changelogs.Add(new TaskChangelog
                    {
                        Created = cl.Created,
                        Field = x.Field,
                        From = x.From,
                        To = x.To,
                        Author = new TaskChangelog.AuthorMeta
                        {
                            AccountId = cl.Author.AccountId,
                            DisplayName = cl.Author.DisplayName,
                            EmailAddress = cl.Author.EmailAddress
                        }
                    }));
                }
                issue.TaskChangelogs = changelogs;
            }

            return issueSearch.Issues;
        }

        #endregion

        #region Private Methods

        private string ReadCloudId(Guid integrationId)
        {
            return ReadField(integrationId, ATConstants.AT_CLOUD_ID, "Unknown cloud id for integration " + integrationId);
        }

        #endregion

    }
}