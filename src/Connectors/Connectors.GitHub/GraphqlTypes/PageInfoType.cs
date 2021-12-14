using Newtonsoft.Json;

namespace Tayra.Connectors.GitHub
{
    public class PageInfoType
    {
        [JsonProperty("endCursor")]
        public string EndCursor { get; set; }

        [JsonProperty("hasNextPage")]
        public bool HasNextPage { get; set; }
    }
}
