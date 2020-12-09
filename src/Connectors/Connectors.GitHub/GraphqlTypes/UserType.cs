using Newtonsoft.Json;

namespace Tayra.Connectors.GitHub
{
    public class UserType
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("isSiteAdmin")]
        public string IsSiteAdmin { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }
    }
}
