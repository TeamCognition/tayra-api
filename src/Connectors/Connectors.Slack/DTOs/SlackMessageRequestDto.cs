using Newtonsoft.Json;

namespace Tayra.Connectors.Slack.DTOs
{
    public class SlackMessageRequestDto
    {
        [JsonProperty("channel")] public string Channel { get; set; }
        [JsonProperty("text")] public string Text { get; set; }
        [JsonProperty("blocks")] public string Blocks { get; set; }
        [JsonProperty("attachments")] public string Attachments { get; set; }

        
    }
}