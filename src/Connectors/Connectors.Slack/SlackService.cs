using RestSharp;

namespace Tayra.Connectors.Slack
{
    public static class SlackService
    {
        #region Constants
        
        public const string CLIENT_ID = "698826045604.1130569857540";
        public const string CLIENT_SECRET = "14642ab9d50d2c898ef0720b4072c7ab";
        
        private const string BASE_REST_URL = "https://slack.com/api";
        private const string ACCESS_TOKEN_URL = "/oauth.v2.access";
        
        
        //developer.github.com/apps/building-github-apps/identifying-and-authorizing-users-for-github-apps/#identifying-users-on-your-site
        public static IRestResponse<TokenResponse> GetUserAccessToken(string code, string redirectUrl)
        {
            var request = new RestRequest(ACCESS_TOKEN_URL, Method.POST);
            request.AddParameter("client_id", CLIENT_ID);
            request.AddParameter("client_secret", CLIENT_SECRET);
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", redirectUrl); //must match the originally submitted URI (if one was sent) [we are not sending it]

            request.RequestFormat = DataFormat.Json;

            var client = new RestClient(BASE_REST_URL);
            return client.Execute<TokenResponse>(request);
        }
        #endregion
    }
}