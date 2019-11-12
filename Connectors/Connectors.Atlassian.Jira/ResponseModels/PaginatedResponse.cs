using System.Collections.Generic;
using RestSharp.Deserializers;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class PaginatedResponse<T> where T : class
    {
        [DeserializeAs(Name = "nextPage")]
        public string NextPage { get; set; }

        [DeserializeAs(Name = "maxResults")] //doesn't work?
        public int ResultCount { get; set; }

        [DeserializeAs(Name = "startAt")]
        public int StartAt { get; set; }

        [DeserializeAs(Name = "total")]
        public int Total { get; set; }

        [DeserializeAs(Name = "isLast")]
        public bool IsLast { get; set; }

        [DeserializeAs(Name = "values")]
        public List<T> Values { get; set; }

    }
}