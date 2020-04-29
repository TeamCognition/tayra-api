using System;
using System.Collections.Generic;
using RestSharp.Deserializers;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class GetAccessibleResourcesResponse
    {
        [DeserializeAs(Name = "id")]
        public string CloudId { get; set; }

        [DeserializeAs(Name = "url")]
        public string Url { get; set; }

        [DeserializeAs(Name = "name")]
        public string Name { get; set; }

        [DeserializeAs(Name = "scopes")]
        public List<string> Scopes { get; set; }

        [DeserializeAs(Name = "avatarUrl")]
        public string AvatarUrl { get; set; }
    }
}
