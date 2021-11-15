using Newtonsoft.Json;

namespace Tayra.Connectors.Slack.DTOs
{
    public class ResponseMetadata
    {
        [JsonProperty("next_cursor")]
        public string NextCursor { get; set; }
    }
}