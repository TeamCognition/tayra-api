﻿using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using Tayra.Connectors.Common;

namespace Tayra.Connectors.Atlassian.Jira
{
    public static class AtlassianJiraService
    {
        #region Constants
        //NON-PRODUCTION KEYS
        //public const string APP_ID = "nWAFSvAyF83nkGYVtJZxpeutVjZA34gd";
        //public const string APP_SECRET = "bhCvqhHtcnnx-JMaHkpkUXZ2UsQvCnXhVFGOJyvVlFapdiHXQ_ETht0e6LdHzpvq";

        private const string BASE_AUTH_URL = "https://auth.atlassian.com/oauth";
        private const string ACCESS_TOKEN_URL = "token";

        private const string BASE_AT_URL = "https://api.atlassian.com";
        private const string ACCESSIBLE_RESOURCES = "oauth/token/accessible-resources";

        private const string BASE_URL = "https://api.atlassian.com/ex/jira/{0}";
        private const string API = "rest/api/3/";
        private const string GET_PROJECTS = API + "project/search";
        private const string GET_PROJECT_STATUSES = API + "project/{0}/statuses";
        private const string GET_ISSUE = API + "issue/{0}";
        private const string GET_ISSUE_CHANGELOGS = API + "issue/{0}/changelog";
        private const string GET_ISSUE_TYPES = API + "issuetype";
        private const string GET_USERS = API + "users/search";
        private const string GET_CURRENT_USER = API + "myself";
        private const string CREATE_WEBHOOK = API + "webhook";
        private const string SEARCH_WITH_JQL = API + "search";

        #endregion

        public static IRestResponse<TokenResponse> GetAccessToken(string appId, string appSecret, string authorizationCode, string redirectUrl)
        {
            var request = new RestRequest(ACCESS_TOKEN_URL, Method.POST);
            request.AddParameter("client_id", appId);
            request.AddParameter("client_secret", appSecret);
            request.AddParameter("code", authorizationCode);
            request.AddParameter("redirect_uri", redirectUrl);
            request.AddParameter("grant_type", "authorization_code");

            request.RequestFormat = DataFormat.Json;

            var client = new RestClient(BASE_AUTH_URL);
            return client.Execute<TokenResponse>(request);
        }

        public static IRestResponse<List<GetAccessibleResourcesResponse>> GetAccessibleResources(string tokenType, string accessToken)
        {
            var request = new RestRequest(ACCESSIBLE_RESOURCES, Method.GET);
            request.AddHeader("Authorization", $"{tokenType} {accessToken}");

            request.RequestFormat = DataFormat.Json;

            var client = new RestClient(BASE_AT_URL);
            var resp = client.Execute<List<GetAccessibleResourcesResponse>>(request);

            return resp;
        }

        public static IRestResponse<TokenResponse> RefreshAccessToken(string appId, string appSecret, string refreshToken)
        {
            var request = new RestRequest(ACCESS_TOKEN_URL, Method.POST);
            request.AddParameter("client_id", appId);
            request.AddParameter("client_secret", appSecret);
            request.AddParameter("refresh_token", refreshToken);
            request.AddParameter("grant_type", "refresh_token");

            request.RequestFormat = DataFormat.Json;

            var client = new RestClient(BASE_AUTH_URL);
            return client.Execute<TokenResponse>(request);
        }

        private static IRestResponse<PaginatedResponse<T>> GetPaginatedResponse<T>(IRestClient client, RestRequest request, int startAt = 0) where T : class
        {
            request.AddOrUpdateParameter("startAt", startAt.ToString(), ParameterType.QueryString);

            var response = client.Execute<PaginatedResponse<T>>(request);

            if (!response.Data.IsLast)
            {
                var nextResponse = GetPaginatedResponse<T>(client, request, response.Data.StartAt + response.Data.Values.Count);
                response.Data.Values.AddRange(nextResponse.Data.Values);
            }

            return response;
        }

        //private static IRestResponse<ICollection<T>> GetCollectionResponse<T>(IRestClient client, RestRequest request, int startAt = 0) where T : class
        //{
        //    request.AddOrUpdateParameter("startAt", startAt.ToString(), ParameterType.QueryString);

        //    var response = client.Execute<PaginatedResponse<T>>(request);
        //    //var response = client.Execute<T>(request);
        //    if (!response.Data.IsLast)
        //    {
        //        var nextResponse = GetCollectionResponse<T>(client, request, response.Data.StartAt + response.Data.ResultCount);
        //        response.Data.Values.AddRange(nextResponse.Data);
        //    }

        //    return response;
        //}

        private static IRestResponse<SearchResponse> GetSearchResponse(IRestClient client, RestRequest request, int startAt = 0)
        {
            request.AddOrUpdateParameter("startAt", startAt.ToString(), ParameterType.QueryString);

            var response = client.Execute<SearchResponse>(request);
            if (response.Data.StartAt + response.Data.MaxResults < response.Data.Total)
            {
                var nextResponse = GetSearchResponse(client, request, response.Data.StartAt + response.Data.MaxResults);
                response.Data.Issues.AddRange(nextResponse.Data.Issues);
            }

            return response;
        }

        public static IRestResponse<PaginatedResponse<JiraProject>> GetProjects(string cloudId, string tokenType, string accessToken)
        {
            var request = new RestRequest(GET_PROJECTS, Method.GET);
            request.AddHeader("Authorization", $"{tokenType} {accessToken}");

            var client = new RestClient(string.Format(BASE_URL, cloudId))
                .UseSerializer(() => new JsonNetSerializer());

            return GetPaginatedResponse<JiraProject>(client, request);
        }

        public static IRestResponse<List<JiraIssueType>> GetProjectStatuses(string cloudId, string tokenType, string accessToken, string jiraProjectId)
        {
            var request = new RestRequest(string.Format(GET_PROJECT_STATUSES, jiraProjectId), Method.GET);
            request.AddHeader("Authorization", $"{tokenType} {accessToken}");

            var client = new RestClient(string.Format(BASE_URL, cloudId))
                .UseSerializer(() => new JsonNetSerializer());

            var response = client.Execute<List<JiraIssueType>>(request);

            return response;
        }

        public static IRestResponse<JiraIssue> GetIssue(string cloudId, string tokenType, string accessToken, string issueIdOrKey, string expand = "")
        {
            var request = new RestRequest(string.Format(GET_ISSUE, issueIdOrKey), Method.GET);
            request.AddHeader("Authorization", $"{tokenType} {accessToken}");

            var client = new RestClient(string.Format(BASE_URL, cloudId))
                .UseSerializer(() => new JsonNetSerializer());

            var response = client.Execute<JiraIssue>(request);

            return response;
        }

        public static IRestResponse<PaginatedResponse<JiraIssueChangelog>> GetIssueChangelog(string cloudId, string tokenType, string accessToken, string issueIdOrKey)
        {
            var request = new RestRequest(string.Format(GET_ISSUE_CHANGELOGS, issueIdOrKey), Method.GET);
            request.AddHeader("Authorization", $"{tokenType} {accessToken}");

            var client = new RestClient(string.Format(BASE_URL, cloudId))
                .UseSerializer(() => new JsonNetSerializer());

            return GetPaginatedResponse<JiraIssueChangelog>(client, request);
        }

        public static IRestResponse<JiraUser> GetLoggedInUser(string cloudId, string tokenType, string accessToken)
        {
            var request = new RestRequest(GET_CURRENT_USER, Method.GET);
            request.AddHeader("Authorization", $"{tokenType} {accessToken}");

            var client = new RestClient(string.Format(BASE_URL, cloudId))
                .UseSerializer(() => new JsonNetSerializer());

            return client.Execute<JiraUser>(request);
        }

        public static IRestResponse<List<JiraUser>> GetUsers(string cloudId, string tokenType, string accessToken)
        {
            var request = new RestRequest(GET_USERS, Method.GET);
            request.AddHeader("Authorization", $"{tokenType} {accessToken}");
            request.AddQueryParameter("maxResults", "2000");
            request.AddQueryParameter("startAt", "0");

            var client = new RestClient(string.Format(BASE_URL, cloudId))
                .UseSerializer(() => new JsonNetSerializer());

            return client.Execute<List<JiraUser>>(request);
        }

        public static IRestResponse<List<JiraIssueType>> GetIssueTypes(string cloudId, string tokenType, string accessToken)
        {
            var request = new RestRequest(string.Format(GET_ISSUE_TYPES), Method.GET);
            request.AddHeader("Authorization", $"{tokenType} {accessToken}");

            var client = new RestClient(string.Format(BASE_URL, cloudId))
                .UseSerializer(() => new JsonNetSerializer());

            var response = client.Execute<List<JiraIssueType>>(request);

            return response;
        }

        public static IRestResponse CreateIssueUpdateWebhook(string cloudId, string tokenType, string accessToken)
        {
            var request = new RestRequest(string.Format(CREATE_WEBHOOK), Method.POST);
            request.AddHeader("Authorization", $"{tokenType} {accessToken}");
            request.AddParameter("application/json", JsonConvert.SerializeObject(new
            {
                url = "https://tayra-dev.azurewebsites.net/webhooks/atjissueupdate",
                webhooks = new[]
                {
                    new { events = new string[] { "jira:issue_updated" } }
                }

            }), ParameterType.RequestBody);


            var client = new RestClient(string.Format(BASE_URL, cloudId))
                .UseSerializer(() => new JsonNetSerializer());

            var response = client.Execute(request);

            return response;
        }

        public static IRestResponse<SearchResponse> GetBulkIssuesWithChangelog(string cloudId, string tokenType, string accessToken, int periodInDays, params string[] projects)
        {
            var request = new RestRequest(string.Format(SEARCH_WITH_JQL), Method.GET);
            request.AddHeader("Authorization", $"{tokenType} {accessToken}");
            request.AddQueryParameter("jql", $"project in ({string.Join(',', projects)}) AND (created>=-{periodInDays}d OR updated>=-{periodInDays}d)", false);
            request.AddQueryParameter("maxResults", "100", false);
            request.AddQueryParameter("expand", "changelog", false);//this will only get last 100 changelogs

            var client = new RestClient(string.Format(BASE_URL, cloudId))
                .UseSerializer(() => new JsonNetSerializer());

            var response = GetSearchResponse(client, request);

            return response;
        }
    }
}