using Newtonsoft.Json;
using Tayra.Connectors.Slack.DTOs;

namespace Tayra.Connectors.Slack
{
    public class UsersListResponse
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }

        [JsonProperty("members")]
        public Member[] Members { get; set; }

        [JsonProperty("cache_ts")]
        public long CacheTs { get; set; }

        [JsonProperty("response_metadata")]
        public ResponseMetadata ResponseMetadata { get; set; }
        
        public class Member
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("team_id")]
            public string TeamId { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("deleted")]
            public bool Deleted { get; set; }

            [JsonProperty("color")]
            public string Color { get; set; }

            [JsonProperty("real_name")]
            public string RealName { get; set; }

            [JsonProperty("tz")]
            public string Tz { get; set; }

            [JsonProperty("tz_label")]
            public string TzLabel { get; set; }

            [JsonProperty("tz_offset")]
            public long TzOffset { get; set; }

            [JsonProperty("profile")]
            public Profile Profile { get; set; }

            [JsonProperty("is_admin")]
            public bool IsAdmin { get; set; }

            [JsonProperty("is_owner")]
            public bool IsOwner { get; set; }

            [JsonProperty("is_primary_owner")]
            public bool IsPrimaryOwner { get; set; }

            [JsonProperty("is_restricted")]
            public bool IsRestricted { get; set; }

            [JsonProperty("is_ultra_restricted")]
            public bool IsUltraRestricted { get; set; }

            [JsonProperty("is_bot")]
            public bool IsBot { get; set; }

            [JsonProperty("updated")]
            public long Updated { get; set; }

            [JsonProperty("is_app_user", NullValueHandling = NullValueHandling.Ignore)]
            public bool? IsAppUser { get; set; }

            [JsonProperty("has_2fa")]
            public bool Has2Fa { get; set; }
        }
    }
}