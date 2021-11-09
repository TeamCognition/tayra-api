using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tayra.Common;
using Tayra.Connectors.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.GitHub
{
    public class GitHubConnector : BaseOAuthConnector
    {
        private const string CONFIG_APP_ID = "Connectors:Github:AppId";
        private const string CONFIG_APP_RSAKEY = "Connectors:Github:AppRsaKey";
        private const string CONFIG_CLIENT_ID = "Connectors:Github:ClientId";
        private const string CONFIG_CLIENT_SECRET = "Connectors:Github:ClientSecret";
        private const string CONFIG_APP_NAME = "Connectors:Github:AppName";
        private const string SEGMENT_AUTH_URL = "https://github.com/apps";
        private const string PROFILE_AUTH_URL = "https://github.com/login/oauth/authorize";

        public GitHubConnector(ILogger logger, OrganizationDbContext dataContext, CatalogDbContext catalogDbContext, IConfiguration config) : base(logger, dataContext, catalogDbContext, config)
        {
        }

        public GitHubConnector(ILogger logger, IHttpContextAccessor httpContext, OrganizationDbContext dataContext, CatalogDbContext catalogDbContext, IConfiguration config) : base(logger, httpContext, dataContext, catalogDbContext, config)
        {
        }

        public override IntegrationType Type { get; } = IntegrationType.GH;

        #region Public Methods

        public override string GetAuthUrl(OAuthState state)
        {
            return state.IsSegmentAuth
                ? $"{SEGMENT_AUTH_URL}/{Config[CONFIG_APP_NAME]}/installations/new?state={state}"
                : $"{PROFILE_AUTH_URL}?client_id={Config[CONFIG_CLIENT_ID]}&state={state}";
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

                var userTokenData = GitHubService.GetUserAccessToken(Config[CONFIG_CLIENT_ID], Config[CONFIG_CLIENT_SECRET], code, GetCallbackUrl(state.ToString()))?.Data;
                var loggedInUser = GitHubService.GetLoggedInUser(userTokenData.TokenType, userTokenData.AccessToken);

                var profileIntegration = OrganizationContext.Integrations.Include(x => x.Fields).OrderByDescending(x => x.Created).FirstOrDefault(x => x.ProfileId == state.ProfileId && x.Type == Type);
                var segmentIntegration = OrganizationContext.Integrations.Include(x => x.Fields).OrderByDescending(x => x.Created).FirstOrDefault(x => x.SegmentId == state.SegmentId && x.ProfileId == null && x.Type == Type);
                if (segmentIntegration == null && !state.IsSegmentAuth)
                {
                    throw new CogSecurityException($"profileId: {state.ProfileId} tried to integrate {Type} before segment integration");
                }

                if (loggedInUser != null)
                {
                    var profileFields = new Dictionary<string, string>
                    {
                        [Constants.PROFILE_EXTERNAL_ID] = loggedInUser.Login
                    };

                    CreateProfileIntegration(state.ProfileId, state.SegmentId, installationId: null, profileFields, profileIntegration);
                }

                if (state.IsSegmentAuth && userTokenData != null)
                {
                    var installations = GitHubService.GetUserInstallations(userTokenData.TokenType, userTokenData.AccessToken);
                    var installation = Utils.FindTayraAppInstallation(installations?.Data.Installations, Config[CONFIG_APP_ID]);
                    var installationId = installation.Id;
                    var installationToken = GitHubService.GetInstallationAccessToken(installationId, Config[CONFIG_APP_ID], Config[CONFIG_APP_RSAKEY])?.Data.AccessToken;

                    var repositories = GitHubService.GetInstallationRepositories(installationToken)?.Data?.Repositories;

                    AddOrUpdateRepositoriesByIntegration(installationId, repositories);
                    AddOrUpdateWebhooks(installationId, installationToken, repositories);

                    var targetName = installation.TargetType == "Organization"
                        ? Utils.GetInstallationOrganizationName(userTokenData.AccessToken, installation.TargetId)
                        : loggedInUser?.Login;

                    var segmentFields = new Dictionary<string, string>
                    {
                        [Constants.ACCESS_TOKEN] = userTokenData.AccessToken,
                        [Constants.ACCESS_TOKEN_TYPE] = userTokenData.TokenType,
                        [Constants.SCOPE] = userTokenData.Scope,
                        [GHConstants.GH_INSTALLATION_ID] = installationId,
                        [GHConstants.GH_INSTALLATION_TARGET_TYPE] = installation.TargetType,
                        [GHConstants.GH_INSTALLATION_TARGET_NAME] = targetName
                    };

                    segmentIntegration = CreateSegmentIntegration(state.SegmentId, installationId, segmentFields, segmentIntegration);
                }

                var unlinkedGitCommits = OrganizationContext.GitCommits.Where(x => x.AuthorProfileId == null && x.AuthorExternalId == loggedInUser.Id);
                foreach (var commit in unlinkedGitCommits)
                {
                    commit.AuthorProfileId = state.ProfileId;
                }

                OrganizationContext.SaveChanges();
                CatalogContext.SaveChanges();
                return segmentIntegration;
            }
            return null;
        }

        public override void UpdateAuthentication(string installationId)
        {
            var installationToken = GitHubService.GetInstallationAccessToken(installationId, Config[CONFIG_APP_ID], Config[CONFIG_APP_RSAKEY])?.Data.AccessToken; ;

            var repositories = GitHubService.GetInstallationRepositories(installationToken)?.Data?.Repositories;

            AddOrUpdateRepositoriesByIntegration(installationId, repositories);
            AddOrUpdateWebhooks(installationId, installationToken, repositories);

            OrganizationContext.SaveChanges();
        }

        public List<CommitType> GetCommitsByPeriodFromAllBranches(Guid integrationId, DateTime since, (string name, string owner) repository)
        {
            var accessToken = ReadAccessToken(integrationId);
            var accessTokenType = ReadAccessTokenType(integrationId);
            var branches = GitHubService.GetBranchesByRepository(accessToken, repository.name, repository.owner);

            List<CommitType> commitsFromAllBranches = new List<CommitType>();
                                     
            foreach (var branch in branches)
            {
                var commitsByPeriod = GitHubService.GetCommitsByPeriod(accessTokenType, accessToken, since, repository.owner, repository.name, branch);
               commitsFromAllBranches.AddRange(commitsByPeriod);
            }

            var uniqueCommitsFromAllBranches = commitsFromAllBranches.GroupBy(x => x.Sha).Select(x => x.FirstOrDefault())
                                                                     .ToList();

            return uniqueCommitsFromAllBranches;
        }

        public GetPullRequestsResponse GetPullRequestsByPeriod(Guid integrationId, string repositoryName, string repositoryOwner)
        {
            var accessToken = ReadAccessToken(integrationId);
            var accessTokenType = ReadAccessTokenType(integrationId);
            return GitHubService.GetPullRequestsWithReviews(accessTokenType, accessToken, repositoryName, repositoryOwner);
        }

        #endregion

        public GetRepositoriesResponse.Repository[] GetRepositories(string installationId)
        {
            var installationToken = GitHubService.GetInstallationAccessToken(installationId, Config[CONFIG_APP_ID], Config[CONFIG_APP_RSAKEY])?.Data.AccessToken; ;
            
            return GitHubService.GetInstallationRepositories(installationToken)?.Data?.Repositories;
        }
        
        #region Private Methods

        public void AddOrUpdateRepositoriesByIntegration(string installationId, GetRepositoriesResponse.Repository[] dto) //RepositoryAddOrUpdate
        {
            var repos = OrganizationContext.Repositories.Where(x => x.IntegrationInstallationId == installationId).ToArray();

            var dtoRepoIds = dto.Select(x => x.ExternalId);
            OrganizationContext.RemoveRange(repos.Where(x => !dtoRepoIds.Contains(x.ExternalId)));

            foreach (var r in dto)
            {
                var repo = repos.FirstOrDefault(x => x.ExternalId == r.ExternalId);

                if (repo == null)
                {
                    repo = new Repository
                    {
                        IntegrationInstallationId = installationId,
                        ExternalId = r.ExternalId,
                        ExternalUrl = r.ExternalUrl
                    };

                    OrganizationContext.Add(repo);
                }

                repo.Name = r.Name;
                repo.NameWithOwner = r.FullName;
            }
        }

        private void AddOrUpdateWebhooks(string installationId, string installationToken, GetRepositoriesResponse.Repository[] dto)
        {
            var repos = OrganizationContext.Repositories.Where(x => x.IntegrationInstallationId == installationId).ToArray();

            var dtoIds = dto.Select(x => x.ExternalId);
            var idsToRemove = repos.Where(x => !dtoIds.Contains(x.ExternalId));

            foreach (var r in dto)
            {
                var repo = repos.FirstOrDefault(x => x.ExternalId == r.ExternalId);

                if (repo == null)
                {
                    GitHubService.CreateRepositoryWebhook(installationToken, r.Owner.Login, r.Name, TenantInfo.Identifier);
                }

                //update here if needed
            }
        }

        #endregion

    }
}