using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class JiraIssueChangelog
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("author")]
        public JiraUser Author { get; set; }

        [JsonProperty("items")]
        public ICollection<Item> Items { get; set; }

        public class Item
        {
            [JsonProperty("field")]
            public string Field { get; set; }

            [JsonProperty("fieldtype")]
            public string FieldType { get; set; }

            [JsonProperty("fieldId")]
            public string FieldId { get; set; }

            [JsonProperty("from")]
            public string From { get; set; }

            [JsonProperty("fromString")]
            public string FromStr { get; set; }

            [JsonProperty("to")]
            public string To { get; set; }

            [JsonProperty("toString")]
            public string ToStr { get; set; }
        }
    }
}
