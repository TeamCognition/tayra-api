using RestSharp;

namespace Tayra.Connectors.GitHub
{
    public static class GitHubService
    {
        #region Constants
        public const string APP_ID = "WTORmTncLPxiByA5UzO0DqMHvjwFBKat";
        public const string APP_SECRET = "B5-DtqBFoexwA0MprGkRVCdp2XgEE6L8dxBoa5d4QLuVpu8CkyHr030WinYMv9wp";
        //NON-PRODUCTION KEYS
        //public const string APP_ID = "nWAFSvAyF83nkGYVtJZxpeutVjZA34gd";
        //public const string APP_SECRET = "bhCvqhHtcnnx-JMaHkpkUXZ2UsQvCnXhVFGOJyvVlFapdiHXQ_ETht0e6LdHzpvq";

        private const string BASE_AUTH_URL = "https://auth.atlassian.com/oauth";
        private const string ACCESS_TOKEN_URL = "token";

        private const string BASE_URL = "https://api.atlassian.com/ex/jira/{0}";
        private const string API = "rest/api/3/";
        private const string GET_PROJECTS = API + "project/search";

        public static IRestResponse<TokenResponse> GetAccessToken(string authorizationCode, string redirectUrl)
        {
            var request = new RestRequest(ACCESS_TOKEN_URL, Method.POST);
            request.AddParameter("client_id", APP_ID);
            request.AddParameter("client_secret", APP_SECRET);
            request.AddParameter("code", authorizationCode);
            request.AddParameter("redirect_uri", redirectUrl);
            request.AddParameter("grant_type", "authorization_code");

            request.RequestFormat = DataFormat.Json;

            var client = new RestClient(BASE_AUTH_URL);
            return client.Execute<TokenResponse>(request);
        }

        public static IRestResponse<TokenResponse> RefreshAccessToken(string refreshToken)
        {
            var request = new RestRequest(ACCESS_TOKEN_URL, Method.POST);
            request.AddParameter("client_id", APP_ID);
            request.AddParameter("client_secret", APP_SECRET);
            request.AddParameter("refresh_token", refreshToken);
            request.AddParameter("grant_type", "refresh_token");

            request.RequestFormat = DataFormat.Json;

            var client = new RestClient(BASE_AUTH_URL);
            return client.Execute<TokenResponse>(request);
        }
        #endregion
    }
}
