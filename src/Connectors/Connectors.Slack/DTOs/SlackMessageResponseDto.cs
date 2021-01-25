using Newtonsoft.Json;
using Tayra.Connectors.Slack.ResponseModels;

namespace Tayra.Connectors.Slack.DTOs
{
    public class SlackMessageResponseDto : SlackBaseResponse
    {

        [JsonProperty("channel")]
        public string Channel { get; set; }
        
        [JsonProperty("message")]
        public ResMessage Message { get; set; }
        
        public class ResMessage
        {
            [JsonProperty("user")]
            public string User { get; set; }
            [JsonProperty("text")]
            public string Text { get; set; }

        }
        
    }
}