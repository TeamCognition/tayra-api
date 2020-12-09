using System;
using Newtonsoft.Json;

namespace Tayra.Models.Organizations.Metrics.GraphqlTypes
{
    public class CommitType
    {
        [JsonProperty("oid")]
        public String OId { get; set; }
        
        [JsonProperty("message")]
        public String Message { get; set; }
        
        [JsonProperty("additions")]
        public int Additions { get; set; }
        
        [JsonProperty("deletions")]
        public int Deletions { get; set; }
        
        [JsonProperty("author")] 
        public GitHubUser Author { get; set; }

        public class GitHubUser
        {
            [JsonProperty("name")]
            public String Name { get; set; }
        
            [JsonProperty("email")]
            public String email { get; set; }

        }
    }
}