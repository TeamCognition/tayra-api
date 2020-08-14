using System;
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
        private const string AUTH_URL = "https://github.com/apps/tayra-app/installations/new";
        private const string OAUTH_APP_AUTH_URL = "https://github.com/login/oauth/authorize?client_id=Iv1.8aa19d523bcef4dd";
        
        private const string GHAPP_RSA_KEY = "MIIEpAIBAAKCAQEAuX4G+VnktB8RG+1NW8mwR5HTdqyHHUAKL6ZakGRZ2ysS2J9vW8IzS3yJuE74bDuGxSHXXVVdRvs5ezbUFR7gF6hkqvrKw6J5oxOVblvZcQl/lG5VWVuwshJvowa8E6E0ihpNZ+xvyfDvNzXgdqwi44hWnw5U3p6bORc/5gK6i9C5h3vDoM2QLNh6K1RyVp5fPftN126v5OZxbZUTGreqmdjQQCQklgrxqSqD/yB8p54z17t125yU2yX6kEhP0//OlqxblZVBqXf0/j3FOWX1EupFLvF4vXbYjmXus5yoKwLUEnvmkOmEKhyPvR2Q0kQGTlCMY/5bKc3Px1U1StTI1QIDAQABAoIBAFO5c9Jm5dj7UNCnKsysW5niU210cEQenLpnPud/tCM97PLD/BKRtG91FgCP/Id10t316WyiVEjuqkJYPCAQYJutEUsviggFtRuLgl5erOXdoK1Ro8qCnV2y/pq6NngxwjI3rwqiaM7gpkjoU5mdFt5Wsqp2YI5fTrbVLK1YO/VRCzmFUbzYxdQrYdVj93GRlBFuptxXE92TS6DsH60TETWZdV45qntsx0YJcHBZpTDzDrjNvVJJqVaH8jYfLC90wo3p2cENxV23fWNd+ac+jeQMEndX4AV1xAvG883snLxTTvSDbKBGC9DUr1a/73uQcEI7qMEQqACjDYd31VXOIeECgYEA3p1P0SYqSS3d+81SN5geuGXREzDOHbfW5HlmqBFuc5ckh6yMDbJwH3GwQ5Z7vewF9lsyJ6Pu1kXu1R1c8AW2cqP9jsacgM2UHffAqFzaI3+EWwdszNcl6zxndQt+l0o+JdpviSia5d7lj+JXxx6XrfLlwJGjxJw5kxwMgB3Cme0CgYEA1U+B971HhzyWAEBg4cY3zsQiBbsCJ23uZzpwM8KZxj/J2Z11qChi2S9YVx6FAWHf2RB1asawhpRxVuGRmf3BXe7AE6Kj69TdMU6nKdCgpe1rYTKwGFnruqL2eUSxwLthudfT3EE10sG0MI6CZ31Xwq98HQSP3xcM8/pYfqPw7YkCgYEAhzOttU3jorxLtNGXnJI0HjQgTfJ3TI9J4UtmMK8dkPB7zDbcfLkh5ccLkZEEqG1/lYb/qBmlRdgFXMPPnSsrCudUaPFxPb0dtzGwfdCe365juVGCH8qPihYOk4Seps39fsnysa/Km8/LRp7mRtXqs0fxiAosF432XcVMRkdM2GUCgYEAiwT+V09sxp6dxBwxB/P/eyooYkO266uhrHVRmupA/gukqccNX0Ky6YkJsf2aAYSgNv+bBrPnaE5mb5EjK5FN7MIlPKbK3nAkmHYCTCZEDN/nE7nNOpGgKEr9B5vVnR6CWnRnBy0YvvqvTNYT9w6hm6hy4xaODX8gWgHWmvKNmsECgYAvOi9AI4g0tu6U/eJYTkimdeWRpo1RJIKqwm37lOFBMmU1kHu/mFTKIEkHlJYckYgzufLcThT2zeRrl7OtMC8pZMOhsDi0L5pCk/x9ruFsiIPkxHq6b7eRjioj/hy8buQwz2J66Yoj8hpV36gOwFj7dpr7wwpbqqypvQee97oJpg==";
        private const string GHAPP_ID = "63108";
        public GitHubConnector(ILogger logger, OrganizationDbContext dataContext) : base(logger, dataContext)
        {
        }

        public GitHubConnector(ILogger logger, IHttpContextAccessor httpContext, ITenantProvider tenantProvider, OrganizationDbContext dataContext) : base(logger, httpContext, tenantProvider, dataContext)
        {
        }

        public override IntegrationType Type { get; } = IntegrationType.GH;

        #region Public Methods

        public override string GetAuthUrl(OAuthState state)
        {
            return state.IsSegmentAuth
                ? $"{AUTH_URL}?state={state}"
                : $"{OAUTH_APP_AUTH_URL}&state={state}";
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

                var userTokenData = GitHubService.GetUserAccessToken(code, state.IsSegmentAuth, GetCallbackUrl(state.ToString()))?.Data;
                var loggedInUser = GitHubService.GetLoggedInUser(userTokenData.TokenType, userTokenData.AccessToken);
                
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
                        [Constants.PROFILE_EXTERNAL_ID] = loggedInUser.Login
                    };
                
                    CreateProfileIntegration(state.ProfileId, state.SegmentId, profileFields, profileIntegration);
                }
                
                if (state.IsSegmentAuth && userTokenData != null)
                {
                    var installations = GitHubService.GetUserInstallations(userTokenData.TokenType, userTokenData.AccessToken);
                    var installationId = Utils.FindTayraAppInstallation(installations?.Data.Installations, GHAPP_ID);
                    var installationToken = GitHubService.GetInstallationAccessToken(installationId, GHAPP_ID, GHAPP_RSA_KEY)?.Data.AccessToken;;

                    var repositories = GitHubService.GetInstallationRepositories(installationToken)?.Data?.Repositories;

                    AddOrUpdateAllByIntegration(installationId, repositories.Select(x => new RepositoryAddOrUpdate
                    {    
                        Name = x.Name,
                        ExternalId = x.Id,
                        ExternalUrl = x.Url,
                        NameWithOwner = x.FullName
                    }).ToArray());
                    
                    var segmentFields = new Dictionary<string, string>
                    {
                        [Constants.ACCESS_TOKEN] = userTokenData.AccessToken,
                        [Constants.ACCESS_TOKEN_TYPE] = userTokenData.TokenType,
                        [Constants.SCOPE] = userTokenData.Scope,
                        [GHConstants.GH_INSTALLATION_ID] = installationId
                    };
                
                    segmentIntegration = CreateSegmentIntegration(state.SegmentId, segmentFields, segmentIntegration);
                }

                var unlinkedGitCommits = OrganizationContext.GitCommits.Where(x => x.AuthorProfileId == null && x.AuthorExternalId == loggedInUser.Id);
                foreach (var commit in unlinkedGitCommits)
                {
                    commit.AuthorProfileId = state.ProfileId;
                }

                OrganizationContext.SaveChanges();
                return segmentIntegration;
            }
            return null;
        }
        
        #endregion

        #region Private Methods

        private void AddOrUpdateAllByIntegration(string installationId, RepositoryAddOrUpdate[] dto) //RepositoryAddOrUpdate
        {
            var repos = OrganizationContext.Repositories.Where(x => x.IntegrationInstallationId == installationId).ToArray();

            var dtoIds = dto.Select(x => x.ExternalId);
            OrganizationContext.RemoveRange(repos.Where(x => !dtoIds.Contains(x.ExternalId)));

            foreach (var r in dto)
            {
                var repo = repos.FirstOrDefault(x => x.ExternalId == r.ExternalId);
                
                if(repo == null)
                {
                    repo = new Repository
                    {
                        IntegrationInstallationId = installationId,
                        ExternalId = r.ExternalId,
                        ExternalUrl = r.ExternalUrl
                    };
                
                    OrganizationContext.Add(repo);
                }
                
                repo.TeamId = r.TeamId;
                repo.Name = r.Name;
                repo.NameWithOwner = r.NameWithOwner;
                repo.PrimaryLanguage = r.PrimaryLanguage;
            }
        }
        
        #endregion
        
    }
}