using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Configuration;
using RestSharp;
using Tayra.Connectors.Common;
using Tayra.Connectors.GitHub.Helper;

namespace Tayra.Connectors.GitHub
{
    public static class GitHubService
    {
        #region Constants
        private const string BASE_AUTH_URL = "https://github.com/login/oauth";
        private const string ACCESS_TOKEN_URL = "/access_token";

        private const string GRAPHQL_URL = "https://api.github.com/graphql";
        private const string BASE_REST_URL = "https://api.github.com";

        //List app installations accessible to the user access token
        private const string GET_USER_INSTALLATIONS = "/user/installations";
        private const string GET_INSTALLATION_TOKEN = "/app/installations/{0}/access_tokens";
        private const string GET_INSTALLATION_REPOSITORIES = "/installation/repositories";
        private const string CREATE_REPOSITORY_WEBHOOK = "/repos/{0}/{1}/hooks";
        private const string GET_LIST_ORGANIZATIONS = "/organizations";
        //developer.github.com/apps/building-github-apps/identifying-and-authorizing-users-for-github-apps/#identifying-users-on-your-site
        public static IRestResponse<TokenResponse> GetUserAccessToken(string clientId, string clientSecret, string authorizationCode, string redirectUrl)
        {
            var request = new RestRequest(ACCESS_TOKEN_URL, Method.POST);
            request.AddParameter("client_id", clientId);
            request.AddParameter("client_secret", clientSecret);
            request.AddParameter("code", authorizationCode);
            request.AddParameter("redirect_uri", redirectUrl);
            //request.AddParameter("state", The unguessable random string you provided in Step 1.);

            request.RequestFormat = DataFormat.Json;

            var client = new RestClient(BASE_AUTH_URL);
            return client.Execute<TokenResponse>(request);
        }

        //docs.github.com/en/rest/reference/apps#create-an-installation-access-token-for-an-app
        public static IRestResponse<InstallationTokenResponse> GetInstallationAccessToken(string installationId, string githubAppId, string rsaPrivateKey)
        {
            var appJwt = Utils.CreateGithubAppJwt(githubAppId, rsaPrivateKey);
            var request = new RestRequest(string.Format(GET_INSTALLATION_TOKEN, installationId), Method.POST);
            request.AddHeader("Authorization", $"Bearer {appJwt}");
            request.AddHeader("Accept", "application/vnd.github.machine-man-preview+json");

            request.RequestFormat = DataFormat.Json;

            var client = new RestClient(BASE_REST_URL);
            return client.Execute<InstallationTokenResponse>(request);
        }
        //developer.github.com/apps/building-github-apps/refreshing-user-to-server-access-tokens/
        public static IRestResponse<TokenResponse> RefreshAccessToken(string clientId, string clientSecret, string refreshToken)
        {
            var request = new RestRequest(ACCESS_TOKEN_URL, Method.POST);
            request.AddParameter("client_id", clientId);
            request.AddParameter("client_secret", clientSecret);
            request.AddParameter("refresh_token", refreshToken);
            request.AddParameter("grant_type", "refresh_token");

            request.RequestFormat = DataFormat.Json;

            var client = new RestClient(BASE_AUTH_URL);
            return client.Execute<TokenResponse>(request);
        }

        public static UserType GetLoggedInUser(string tokenType, string accessToken)
        {
            using var graphQLClient = new GraphQLHttpClient(GRAPHQL_URL, new NewtonsoftJsonSerializer());
            graphQLClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"{tokenType} {accessToken}");

            var viewerRequest =
                new GraphQLRequest
                {
                    Query = @"
                    {
                        viewer {
                            id
                            isSiteAdmin
                            login
                        }
                    }"
                };


            return graphQLClient.SendQueryAsync(viewerRequest, () => new { Viewer = new UserType() }).GetAwaiter()
                .GetResult()?.Data?.Viewer;
        }

        public static List<CommitType> GetCommitsByPeriod(string tokenType, string token, int days, string repositoryOwner, string repositoryName, string repositoryBranch)
        {
            DateTime now = DateTime.UtcNow;
            if (days == 0)
            {
                now = now.Date;
            }
            string preparePeriod = now.AddDays(-days).ToString("o");
            using var graphQLClient = new GraphQLHttpClient(GRAPHQL_URL, new NewtonsoftJsonSerializer());
            graphQLClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"{tokenType} {token}");
            var commitsRequest = new GraphQLRequest
            {
                Query = @"
                    query CommitsByPeriod($commitPeriod : GitTimestamp!,$repositoryName : String!, $repositoryOwner : String!,$repositoryBranch : String! ) {
                        repository(name: $repositoryName owner: $repositoryOwner) {
                            ref(qualifiedName:$repositoryBranch) {
                                target {
                                    ... on Commit {
                                        history(since:$commitPeriod) {
                                            edges {
                                                node {
                                                    oid,
                                                    author {
                                                        name,email
                                                    },
                                                    message
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                ",
                OperationName = "CommitsByPeriod",
                Variables = new
                {
                    commitPeriod = preparePeriod,
                    repositoryOwner = repositoryOwner,
                    repositoryName = repositoryName,
                    repositoryBranch = repositoryBranch

                }
            };
            var gitGraphQlResponse =
                graphQLClient.SendQueryAsync<GetCommitsResponse>(commitsRequest).GetAwaiter().GetResult();
            return MapGQResponse<CommitType>.MapResponseToCommitType(gitGraphQlResponse.Data.Repository.Branch.Target.History.Edges);
        }

        public static List<PullRequestType> GetPullRequestsByPeriod(string tokenType, string token, int days, string repositoryOwner)
        {
            DateTime now = DateTime.UtcNow;
            if (days == 0)
            {
                now = now.Date;
            }
            string preparePeriod = now.AddDays(-days).ToString(("o"));

            using var graphQLClient = new GraphQLHttpClient(GRAPHQL_URL, new NewtonsoftJsonSerializer());
            graphQLClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"{tokenType} {token}");
            var pullRequestsRequest = new GraphQLRequest
            {
                Query = $@"
                    {{
                    search(query:"" org:{repositoryOwner} is:pr created:>{preparePeriod}"", type: ISSUE, last: 100) {{
                      edges {{
                          node {{
                             ... on PullRequest {{
                                      id
                                      url
                                      title
                                      state
                                      createdAt
                                      updatedAt
                                      merged
                                      mergedAt
                                      mergedBy{{
                                            ... on User{{
                                                login
                                                id
                                                 }}
                                            }}
                                      
                                      author{{
                                            ... on User {{
                                                   id
                                                   login
              
                                                 }}
                                          }}
        
                                       }}
                                   }}
                               }}
                          }}
                     }} "

            };
            var gitGraphQlResponse =
                graphQLClient.SendQueryAsync<GetPullRequestsResponse>(pullRequestsRequest).GetAwaiter().GetResult();
            return MapGQResponse<PullRequestType>.MapResponseToCommitType(gitGraphQlResponse.Data.Search.Edges);
        }

        public static CommitType GetCommitBySha(string sha, string token, string repositoryOwner, string repositoryName)
        {
            var graphQlClient = new GraphQLHttpClient(GRAPHQL_URL, new NewtonsoftJsonSerializer());
            graphQlClient.HttpClient.DefaultRequestHeaders.Add("Authorization",$"bearer {token}");
            var graphQlRequest = new GraphQLRequest
            {
                Query = @"
                     
                query CommitsBySha($repositoryName : String!, $repositoryOwner : String!, $commitSha: GitObjectID!){
                    repository(name: $repositoryName, owner: $repositoryOwner){
                    object(oid: $commitSha){
                ... on Commit{
                oid,
            additions,
            deletions
               }
             }
          }   
        }   ",              
                OperationName = "CommitsBySha",
                Variables = new
                {
                    commitSha = sha,
                    repositoryOwner = repositoryName,
                    repositoryName = repositoryName,
                }
            };
            var graphQlResponse = graphQlClient.SendQueryAsync<GetCommitsByShaResponse>(graphQlRequest).GetAwaiter()
                .GetResult();
            return graphQlResponse?.Data?.Repository?.GhObject;
        }
        
        public static IRestResponse<UserInstallationsResponse> GetUserInstallations(string tokenType, string accessToken)
        {
            var request = new RestRequest(GET_USER_INSTALLATIONS, Method.GET);
            request.AddHeader("Authorization", $"{tokenType} {accessToken}");

            var client = new RestClient(BASE_REST_URL)
                .UseSerializer(() => new JsonNetSerializer());

            return client.Execute<UserInstallationsResponse>(request);
        }

        public static IRestResponse<GetRepositoriesResponse> GetInstallationRepositories(string installationToken)
        {
            var request = new RestRequest(GET_INSTALLATION_REPOSITORIES, Method.GET);
            request.AddHeader("Authorization", $"Bearer {installationToken}");

            var client = new RestClient(BASE_REST_URL)
                .UseSerializer(() => new JsonNetSerializer());

            return client.Execute<GetRepositoriesResponse>(request);
        }

        public static IRestResponse CreateRepositoryWebhook(string installationToken, string owner, string repo, string tenantKey)
        {
            var request = new RestRequest(string.Format(CREATE_REPOSITORY_WEBHOOK, owner, repo), Method.POST);
            request.AddHeader("Authorization", $"Bearer {installationToken}");
            request.AddHeader("Accept", "application/vnd.github.v3+json");
            request.AddJsonBody(new { config = new { url = $"https://api.tayra.io/webhooks/gh?tenant={tenantKey}", content_type = "json" } });

            var client = new RestClient(BASE_REST_URL)
                .UseSerializer(() => new JsonNetSerializer());

            return client.Execute(request);
        }

        public static IRestResponse<List<GetOrganizationsResponse>> GetOrganizations(string userToken, long orgId)
        {
            var request = new RestRequest(GET_LIST_ORGANIZATIONS, Method.GET);
            request.AddHeader("Authorization", $"Bearer {userToken}");
            request.AddHeader("Accept", "application/vnd.github.v3+json");
            request.AddQueryParameter("since", (orgId - 1).ToString());

            var client = new RestClient(BASE_REST_URL)
                .UseSerializer(() => new JsonNetSerializer());

            return client.Execute<List<GetOrganizationsResponse>>(request);
        }
        #endregion
    }
}