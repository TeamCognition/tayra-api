using System;
using Cog.Core;
using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace Tayra.Connectors.Slack
{
    public class TokenResponse
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("bot_user_id")]
        public string BotUserId { get; set; }

        [JsonProperty("app_id")]
        public string AppId { get; set; }

        [JsonProperty("team")]
        public EnterpriseDto Team { get; set; }

        [JsonProperty("enterprise")]
        public EnterpriseDto Enterprise { get; set; }

        [JsonProperty("authed_user")]
        public AuthedUserDto AuthedUser { get; set; }
        
        [DeserializeAs(Name = "incoming_webhook")]
        public IncomingWebhookDto IncomingWebhook { get; set; }

        public class EnterpriseDto
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }
        }
            
        public class AuthedUserDto
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("scope")]
            public string Scope { get; set; }

            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("token_type")]
            public string TokenType { get; set; }
        }
        public class IncomingWebhookDto
        {
            [JsonProperty("url")]
            public string Url { get; set; }
                
            [JsonProperty("channel")]
            public string Channel { get; set; } 
                
            [JsonProperty("configuration_url")]
            public string ConfigurationUrl { get; set; } 
        }
    }
}