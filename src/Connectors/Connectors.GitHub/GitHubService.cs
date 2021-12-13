using System;
using System.Collections.Generic;
using Cog.Core;
using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using RestSharp;
using Tayra.Connectors.Common;
using Tayra.Connectors.GitHub.Common;
using Tayra.Connectors.GitHub.Helper;
using Tayra.Connectors.GitHub.ResponseModels;

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
        #endregion
        
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

        public static GetCommitsPageResponse GetCommitsPageByPeriod(string tokenType, string token, DateTime since, string endCursor, string repositoryOwner, string repositoryName, string repositoryBranch)
        {
            using var graphQLClient = new GraphQLHttpClient(GRAPHQL_URL, new NewtonsoftJsonSerializer());
            graphQLClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"{tokenType} {token}");
            var commitsRequest = new GraphQLRequest
            {
                Query = @"
                    query GetCommitsSinceTimestamp($endCursor: String, $commitsSince: GitTimestamp!, $repositoryName: String!, $repositoryOwner: String!, $repositoryBranch: String!) {
                      repository(name: $repositoryName, owner: $repositoryOwner) {
                        ref(qualifiedName: $repositoryBranch) {
                          target {
                            ... on Commit {
                              history(after: $endCursor, since: $commitsSince) {
                                pageInfo {
                                  endCursor
                                  hasNextPage
                                }
                                edges {
                                  node {
                                    oid
                                    additions
                                    deletions
                                    commitUrl
                                    url
                                    committedDate
                                    associatedPullRequests (first: 10) {
                                                      nodes {
                                                        id
                                                        mergedAt
                                                      }
                                                    }
                                    repository {
                                      databaseId
                                      nameWithOwner
                                    }
                                    author {
                                      name
                                      email
                                      user {
                                        login
                                      }
                                    }
                                    message
                                  }
                                }
                              }
                            }
                          }
                        }
                      }
                    }",
                OperationName = "GetCommitsSinceTimestamp",
                Variables = new
                {
                    commitsSince = since.ToString("o"),
                    endCursor = endCursor,
                    repositoryOwner = repositoryOwner,
                    repositoryName = repositoryName,
                    repositoryBranch = repositoryBranch
                }
            };
            var gitGraphQlResponse =
                graphQLClient.SendQueryAsync<GetCommitsResponse>(commitsRequest).GetAwaiter().GetResult().Data.Repository.Branch.Target.History;

            var commitsPage = new GetCommitsPageResponse
            {
                PageInfo = gitGraphQlResponse.PageInfo,
                Commits = MapGQResponse<CommitType>.MapResponseToCommitType(gitGraphQlResponse.Edges)
            };

            return commitsPage;
        }       

        public static GetPullRequestsPageResponse GetPullRequestsWithReviewsPage(string tokenType, string token, string endCursor, string repositoryName, string repositoryOwner)
        {
            using var graphQLClient = new GraphQLHttpClient(GRAPHQL_URL, new NewtonsoftJsonSerializer());
            graphQLClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"{tokenType} {token}");
            var pullRequestsRequest = new GraphQLRequest
            {
                Query = @"
                    query GetPullRequestsWithReviews($endCursor: String, $repositoryName: String!, $repositoryOwner: String!, $prCount: Int!) {
                      repository(name: $repositoryName, owner: $repositoryOwner) {
                        name
                        pullRequests(after: $endCursor, first: $prCount, orderBy: {field: UPDATED_AT, direction: DESC}) { 
                          pageInfo {
                            endCursor
                            hasNextPage
                          }
                          nodes {
                            id
                            number
                            title
                            bodyText
                            url
                            state
                            additions
                            deletions
                            closedAt
                            merged
                            locked
                            mergedAt
                            closedAt
                            createdAt
                            updatedAt
                            url
                            repository {
                              databaseId
                              nameWithOwner
                            }
                            author {
                              login    
                            }
                            commits {
                              totalCount
                            }
                            reviews(last: 30) {
                              totalCount
                              nodes {
                                author {
                                  login
                                }
                                state
                              }
                            }
                          }
                        }
                      }
                    }",
                OperationName = "GetPullRequestsWithReviews",
                Variables = new
                {
                    repositoryOwner = repositoryOwner,
                    repositoryName = repositoryName,
                    endCursor = endCursor,
                    prCount = 30
                }
            };            
            
            return graphQLClient.SendQueryAsync<GetPullRequestsPageResponse>(pullRequestsRequest).GetAwaiter().GetResult().Data;
        }

        public static CommitType GetCommitBySha(string sha, string token, string repositoryOwner, string repositoryName)
        {
            var graphQlClient = new GraphQLHttpClient(GRAPHQL_URL, new NewtonsoftJsonSerializer());
            graphQlClient.HttpClient.DefaultRequestHeaders.Add("Authorization",$"bearer {token}");
            var graphQlRequest = new GraphQLRequest
            {
                Query = @"
                    query CommitsBySha($repoName : String!, $repoOwner : String!, $commitSha: GitObjectID!) {
                        repository(name: $repoName, owner: $repoOwner) {
                            object(oid: $commitSha) {
                                ... on Commit {
                                    oid,
                                    additions,
                                    deletions
                                }
                            }
                        }   
                    }",              
                OperationName = "CommitsBySha",
                Variables = new
                {
                    commitSha = sha,
                    repoOwner = repositoryOwner,
                    repoName = repositoryName
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
        

        public static List<string> GetBranchesByRepository(string accessToken, string repositoryName, string repositoryOwner)
        {
            var graphQlClient = new GraphQLHttpClient(GRAPHQL_URL, new NewtonsoftJsonSerializer());
            graphQlClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            var graphQlRequest = new GraphQLRequest
            {
                Query = @"
                    query BranchesByRepository($repoName: String!, $repoOwner: String!) {
                      repository(name: $repoName, owner: $repoOwner) {
                        refs(first: 100, refPrefix: ""refs/heads/"") {
                          edges {
                            node {
                              name
                            }
                          }
                        }
                      }
                    }",
                OperationName = "BranchesByRepository",
                Variables = new
                {
                    repoOwner = repositoryOwner,
                    repoName = repositoryName
                }
            };
            var graphQlResponse = graphQlClient.SendQueryAsync<GetBranchesByRepository>(graphQlRequest).GetAwaiter()
                .GetResult();
            var graphQlBranches = graphQlResponse.Data?.Repository?.Refs?.Edges;
            List<string> branches = new List<string>();
            if (graphQlBranches.IsNullOrEmpty())
            {
                return branches;
            }
            foreach (var edge in graphQlBranches)
            {
                branches.Add(edge.Node.Name);
            }
            return branches;
        }
        
        public static List<CommitType> GetCommitsByPullRequest(string tokenType, string token, int pullRequestCount, string repositoryOwner, string repositoryName)
        {
            using var graphQLClient = new GraphQLHttpClient(GRAPHQL_URL, new NewtonsoftJsonSerializer());
            graphQLClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"{tokenType} {token}");
            var commitsRequest = new GraphQLRequest
            {
                Query = @"
                query CommitsByPullRequest($repositoryName : String!, $repositoryOwner : String!,$pullRequestCount : Int!) {
                    repository(name:$repositoryName, owner:$repositoryOwner) {
                        pullRequest(number:$pullRequestCount) {
                            commits(first:100) {
                                edges {
                                    node {
                                        commit {
                                            oid,
                                            author {
                                                name,
                                                email
                                            },
                                            message,
                                            additions,
                                            deletions
                                        }
                                    }
                                }
                            }
                        }
                    }
                }",
                OperationName = "CommitsByPullRequest",
                Variables = new {
                    repositoryOwner = repositoryOwner,
                    repositoryName = repositoryName,
                    pullRequestCount = pullRequestCount
                }
            };
            var gitGraphQlResponse =
                graphQLClient.SendQueryAsync<GitCommitByPrResponse>(commitsRequest).GetAwaiter().GetResult();
            List<Edge<GitCommitByPrResponse.PullRequestCommit>> responseCommits =
                gitGraphQlResponse.Data.Repository.pullRequest.Commits.edges;
            List<CommitType> commitsForRes = new List<CommitType>();
            foreach (var edge in responseCommits)
            {
                commitsForRes.Add(edge.Node.Commit);
            }
            return commitsForRes;
        }
    }
}