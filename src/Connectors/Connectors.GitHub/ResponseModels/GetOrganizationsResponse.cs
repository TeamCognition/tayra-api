using Newtonsoft.Json;

namespace Tayra.Connectors.GitHub
{
    public class GetOrganizationsResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }
            
        [JsonProperty("login")]
        public string Login { get; set; }
    }
}