using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Metrics
{
    public class CommitsMetric : PureMetric
    {
        public CommitsMetric(string name, int value) : base(name, value)
        {

        }
        public MetricShard Create(IEnumerable<GitCommit> commits, int dateId) => new MetricShard(commits.Count(), dateId, this);

        public override object[] GetRawMetrics(OrganizationDbContext db, DatePeriod period, Guid entityId, EntityTypes entityType)
        {
            var profileIds = GetProfileIds(db, entityId, entityType);
            return (from c in db.GitCommits
                    where profileIds.Contains(c.AuthorProfileId.Value)
                    where c.Created.Date >= period.From && c.Created.Date <= period.To
                    select new RawMetric
                    {
                        Author = new TableData.Profile($"{c.AuthorProfile.FirstName} {c.AuthorProfile.LastName}",
                            c.AuthorProfile.Username),
                        Commit = new TableData.ExternalLink(c.Message, c.ExternalUrl),
                        Date = new TableData.DateInSeconds(c.Created),
                        Sha = c.Sha
                    }).ToArray<object>();
        }

        public override Type TypeOfRawMetric => typeof(RawMetric);

        public class RawMetric
        {
            public TableData.Profile Author { get; set; }
            public TableData.ExternalLink Commit { get; set; }
            public TableData.DateInSeconds Date { get; set; }
            public string Sha { get; set; }
        }
    }
}