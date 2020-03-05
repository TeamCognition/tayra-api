using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class JiraIssue
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("expand")]
        public string Expand { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("fields")]
        public IssueFields Fields { get; set; }

        /// <summary>
        /// this is for /search api only
        /// </summary>
        [JsonProperty("changelog")]
        public ChangeLogDTO Changelog { get; set; }

        public class ChangeLogDTO
        {
            public int StartAt { get; set; }

            [JsonProperty("histories")]
            public ICollection<JiraIssueChangelog> Histories { get; set; }
        }

        public class IssueFields
        {
            [JsonProperty("statuscategorychangedate")]
            public DateTime StatusCategoryChangeDate { get; set; }

            [JsonProperty("issuetype")]
            public JiraIssueType IssueType { get; set; }

            [JsonProperty("timespent")]
            public int? Timespent { get; set; }

            [JsonProperty("customfield_10030")]
            public float? StoryPointsCF { get; set; }

            [JsonProperty("project")]
            public JiraProject Project { get; set; }

            [JsonProperty("aggregatetimespent")]
            public long? AggregateTimespent { get; set; }

            [JsonProperty("workratio")]
            public long Workratio { get; set; }

            [JsonProperty("lastViewed")]
            public DateTime? LastViewed { get; set; }

            [JsonProperty("created")]
            public DateTime Created { get; set; }

            [JsonProperty("priority")]
            public JiraPriority Priority { get; set; }

            [JsonProperty("labels")]
            public string[] Labels { get; set; }

            [JsonProperty("timeestimate")]
            public long? Timeestimate { get; set; }

            [JsonProperty("aggregatetimeoriginalestimate")]
            public long? Aggregatetimeoriginalestimate { get; set; }

            [JsonProperty("assignee")]
            public JiraUser Assignee { get; set; }

            [JsonProperty("updated")]
            public DateTime Updated { get; set; }

            [JsonProperty("status")]
            public JiraStatus Status { get; set; }

            [JsonProperty("timeoriginalestimate")]
            public int? TimeOriginalEstimate { get; set; }

            //[JsonProperty("description")]
            //public IssueDescription Description { get; set; }

            //[JsonProperty("timetracking")]
            //public JiraTimetracking Timetracking { get; set; }

            [JsonProperty("aggregatetimeestimate")]
            public long? Aggregatetimeestimate { get; set; }

            [JsonProperty("summary")]
            public string Summary { get; set; }

            [JsonProperty("creator")]
            public JiraUser Creator { get; set; }

            [JsonProperty("reporter")]
            public JiraUser Reporter { get; set; }

            //[JsonProperty("aggregateprogress")]
            //public IssueProgress Aggregateprogress { get; set; }

            //[JsonProperty("progress")]
            //public IssueProgress Progress { get; set; }

            public class IssueProgress
            {
                [JsonProperty("progress")]
                public long ProgressProgress { get; set; }

                [JsonProperty("total")]
                public long Total { get; set; }

                [JsonProperty("percent")]
                public long Percent { get; set; }
            }

            public class IssueDescription
            {
                [JsonProperty("version")]
                public long Version { get; set; }

                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("content")]
                public IEnumerable<DescriptionContent> Content { get; set; }

                public class DescriptionContent
                {
                    [JsonProperty("type")]
                    public string Type { get; set; }

                    [JsonProperty("content")]
                    public IEnumerable<ContentContent> Content { get; set; }

                    public class ContentContent
                    {
                        [JsonProperty("type")]
                        public string Type { get; set; }

                        [JsonProperty("text")]
                        public string Text { get; set; }
                    }
                }
            }

        }
    }
}
