using Newtonsoft.Json;

namespace Tayra.Connectors.GitHub
{
    public class ViewerResp
    {
        [JsonProperty("viewer")]
        public UserType Viewer { get; set; }
    }
}