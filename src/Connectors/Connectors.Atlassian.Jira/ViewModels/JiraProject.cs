using System;
using System.Collections.Generic;
using RestSharp.Deserializers;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class JiraProject
    {
        [DeserializeAs(Name = "id")]
        public string Id { get; set; }

        [DeserializeAs(Name = "key")]
        public string Key { get; set; }

        [DeserializeAs(Name = "name")]
        public string Name { get; set; }

        public ICollection<JiraStatus> Statuses { get; set; }
    }
}
