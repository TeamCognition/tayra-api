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
    public class PullRequestsCreatedMetric : PureMetric
    {
        public PullRequestsCreatedMetric(string name, int value) : base(name, value)
        {
            
        }

        public MetricShard Create(IEnumerable<PullRequest> pullRequests, int dateId) =>
            new MetricShard(pullRequests.Count(), dateId, this);

        public override object[] GetRawMetrics(OrganizationDbContext db, DatePeriod period, Guid entityId, EntityTypes entityType)
        {
            var profileIds = GetProfileIds(db, entityId, entityType);
            return (from p in db.PullRequests
                where profileIds.Contains(p.AuthorProfileId.Value)
                where p.Created.Date >= period.From.Date && p.Created.Date < period.To.Date
                select new RawMetric
                {
                    Author = new TableData.Profile($"{p.AuthorProfile.FirstName} {p.AuthorProfile.LastName}",
                        p.AuthorProfile.Username),
                    PullRequest = new TableData.ExternalLink(p.Title, p.ExternalUrl),
                    Date = new TableData.DateInSeconds(p.Created)
                }).ToArray<object>();
        }

        public override Type TypeOfRawMetric => typeof(RawMetric);

        public class RawMetric
        {
            public TableData.Profile Author { get; set; }
            public TableData.ExternalLink PullRequest { get; set; }
            public TableData.DateInSeconds Date { get; set; }
        }
    }
}