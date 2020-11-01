using Newtonsoft.Json;
using Tayra.Connectors.GitHub.Common;

namespace Tayra.Connectors.GitHub
{
    public class GetCommitsResponse
    {
        [JsonProperty("repository")]
        public GithubRepository Repository { get; set; }

        public class GithubRepository
        {
            [JsonProperty("ref")]
            public Branch Branch { get; set; }
           
        }
        public class Target
        {
            [JsonProperty("history")]
            public History History { get; set; }
            
                    
        }
        public class History
        {
            [JsonProperty("edges")]
            public Edge<CommitType>[] Edges { get; set;}
        }
        
        public class Branch
        {
            [JsonProperty("target")]
            public Target Target { get; set; }
               
        }
    }
}