using Newtonsoft.Json;
using Tayra.Connectors.Slack.ResponseModels;

namespace Tayra.Connectors.Slack.DTOs
{
    public class SlackMessageResponseDto : SlackBaseResponse
    {

        [JsonProperty("channel")]
        public string Channel { get; set; }
        
        [JsonProperty("message")]
        public Message ResMessage { get; set; }
        
        public class Message
        {
            public string Username { get; set; }
            public string Text { get; set; }

        }
        
    }
}