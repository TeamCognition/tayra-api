using Newtonsoft.Json;

namespace Tayra.Connectors.GitHub.WebhookPayloads
{
    public class GitHubWebhookUser
    {

        [JsonProperty("login")]
        public string Username { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }
    }
}