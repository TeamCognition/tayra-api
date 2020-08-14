﻿using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using RestSharp;
using Tayra.Connectors.Common;

namespace Tayra.Connectors.GitHub
{
    public static class GitHubService
    {
        #region Constants

        public const string OAUTH_APP_CLIENT_ID = "Iv1.8aa19d523bcef4dd";
        public const string OAUTH_APP_CLIENT_SECRET = "24bbd04c4a071a1298f1dd96b547d3054ef4534a";
        
        public const string CLEINT_ID = "Iv1.8aa19d523bcef4dd";
        public const string CLIENT_SECRET = "24bbd04c4a071a1298f1dd96b547d3054ef4534a";

        private const string BASE_AUTH_URL = "https://github.com/login/oauth";
        private const string ACCESS_TOKEN_URL = "/access_token";

        private const string GRAPHQL_URL = "https://api.github.com/graphql";
        private const string BASE_REST_URL = "https://api.github.com";

        //List app installations accessible to the user access token
        private const string GET_USER_INSTALLATIONS = "/user/installations";
        private const string GET_INSTALLATION_TOKEN = "/app/installations/{0}/access_tokens";
        private const string GET_INSTALLATION_REPOSITORIES = "/installation/repositories";
        
        //developer.github.com/apps/building-github-apps/identifying-and-authorizing-users-for-github-apps/#identifying-users-on-your-site
        public static IRestResponse<TokenResponse> GetUserAccessToken(string authorizationCode, bool isSegmentAuth, string redirectUrl)
        {
            var request = new RestRequest(ACCESS_TOKEN_URL, Method.POST);
            request.AddParameter("client_id", isSegmentAuth ? CLEINT_ID : OAUTH_APP_CLIENT_ID);
            request.AddParameter("client_secret", isSegmentAuth ? CLIENT_SECRET : OAUTH_APP_CLIENT_SECRET);
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
        public static IRestResponse<TokenResponse> RefreshAccessToken(string refreshToken, bool isSegmentAuth)
        {
            var request = new RestRequest(ACCESS_TOKEN_URL, Method.POST);
            request.AddParameter("client_id", isSegmentAuth ? CLEINT_ID : OAUTH_APP_CLIENT_ID);
            request.AddParameter("client_secret", isSegmentAuth ? CLIENT_SECRET : OAUTH_APP_CLIENT_SECRET);
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


            return graphQLClient.SendQueryAsync(viewerRequest, () => new {Viewer = new UserType()}).GetAwaiter()
                .GetResult()?.Data?.Viewer;
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
        
        #endregion
    }
}