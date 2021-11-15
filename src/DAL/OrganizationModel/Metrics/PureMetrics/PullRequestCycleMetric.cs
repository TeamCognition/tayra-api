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
    public class PullRequestCycleMetric : PureMetric
    {
        public PullRequestCycleMetric(string name, int value) : base(name, value)
        {
        }

        public MetricShard Create(IEnumerable<PullRequest> pullRequests, int dateId)
        {
            float prCyclePeriodInMinutes = 0;
            if (pullRequests.IsNullOrEmpty())
            {
                return new MetricShard(0, dateId, this);
            }
            foreach (var pullRequest in pullRequests)
            {
                TimeSpan cyclePeriod = pullRequest.ExternalCreatedAt - pullRequest.MergedAt.Value;
                prCyclePeriodInMinutes = cyclePeriod.Minutes + prCyclePeriodInMinutes;
            }

            return new MetricShard(prCyclePeriodInMinutes / pullRequests.Count(), dateId, this);
        }
        public override object[] GetRawMetrics(OrganizationDbContext db, DatePeriod period, Guid entityId, EntityTypes entityType)
        {
            var profileIds = GetProfileIds(db, entityId, entityType);
            return (from ps in db.PullRequests
                where profileIds.Contains(ps.AuthorProfileId.Value)
                where ps.Created >= period.From && ps.Created < period.To
                where ps.MergedAt != null
                select new RawMetric
                {
                    Author = new TableData.Profile($"{ps.AuthorProfile.FirstName} {ps.AuthorProfile.LastName}",ps.AuthorProfile.Username),
                    PullRequest = new TableData.ExternalLink(ps.Title,ps.ExternalUrl),
                    Date = new TableData.DateInSeconds(ps.Created)
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