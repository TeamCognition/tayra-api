using Newtonsoft.Json;
using Tayra.Connectors.Slack.DTOs;

#nullable enable
namespace Tayra.Connectors.Slack.ResponseModels
{
    public class SlackBaseResponse
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }
        
        [JsonProperty("error")]
        public string? Error { get; set; }
        
        [JsonProperty("warning")]
        public string? Warning { get; set; }
        
        [JsonProperty("response_metadata")]
        public ResponseMetadata? ResponseMetadata { set; get; }
    }
}