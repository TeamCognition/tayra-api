using System;
using Newtonsoft.Json;

namespace Tayra.Connectors.Slack.DTOs
{
    public class Profile
    {
        [JsonProperty("avatar_hash")]
        public string AvatarHash { get; set; }

        [JsonProperty("status_text", NullValueHandling = NullValueHandling.Ignore)]
        public string StatusText { get; set; }

        [JsonProperty("status_emoji", NullValueHandling = NullValueHandling.Ignore)]
        public string StatusEmoji { get; set; }

        [JsonProperty("real_name")]
        public string RealName { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("real_name_normalized")]
        public string RealNameNormalized { get; set; }

        [JsonProperty("display_name_normalized")]
        public string DisplayNameNormalized { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("image_24")]
        public Uri Image24 { get; set; }

        [JsonProperty("image_32")]
        public Uri Image32 { get; set; }

        [JsonProperty("image_48")]
        public Uri Image48 { get; set; }

        [JsonProperty("image_72")]
        public Uri Image72 { get; set; }

        [JsonProperty("image_192")]
        public Uri Image192 { get; set; }

        [JsonProperty("image_512")]
        public Uri Image512 { get; set; }

        [JsonProperty("team", NullValueHandling = NullValueHandling.Ignore)]
        public string Team { get; set; }

        [JsonProperty("image_1024", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Image1024 { get; set; }

        [JsonProperty("image_original", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ImageOriginal { get; set; }

        [JsonProperty("first_name", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [JsonProperty("last_name", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        [JsonProperty("skype", NullValueHandling = NullValueHandling.Ignore)]
        public string Skype { get; set; }
    }
}