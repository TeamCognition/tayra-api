using System;
using Newtonsoft.Json;

namespace Tayra.Connectors.GitHub.WebhookPayloads
{
    public class PushWebhookPayload : BaseWebhookPayload
    {
        [JsonProperty("commits")]
        public CommitDTO[] Commits { get; set; }

        public class CommitDTO
        {
            /// <summary>
            /// sha
            /// </summary>
            [JsonProperty("Id")]
            public string Id { get; set; }

            [JsonProperty("distinct")]
            public bool Distinct { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("timestamp")]
            public DateTime Timestamp { get; set; }

            [JsonProperty("author")]
            public AuthorDTO Author { get; set; }

            public class AuthorDTO
            {
                [JsonProperty("username")]
                public string Username { get; set; }
            }
        }
    }
}