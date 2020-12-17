using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Common;
using Tayra.Models.Organizations.Metrics.GraphqlTypes;

namespace Tayra.Models.Organizations.Metrics.PureMetrics
{
    public class PullRequestSizeMetric : PureMetric
    {
        public PullRequestSizeMetric(string name, int value) : base(name, value)
        {
        }

        public MetricShard Create(MetricService metricService, IEnumerable<PullRequest> pullRequests, int dateId)
        {
            int pullRequestChanges = 0;
            foreach (var pullRequest in pullRequests)
            {
                Guid integrationId = metricService.GetIntegrationId(IntegrationType.GH);
                List<CommitType> commits = MetricService.GetCommitsByPUllRequest("bearer", metricService.ReadAccessToken(integrationId), pullRequest.ExternalNumber);
               foreach (var commit in commits)
               {
                   pullRequestChanges = commit.Additions + commit.Deletions;
               }
            }
            return new MetricShard(pullRequestChanges / pullRequests.Count(), dateId, this);
        }
        public override object[] GetRawMetrics(OrganizationDbContext db, DatePeriod period, Guid entityId, EntityTypes entityType)
        {
            var profileIds = GetProfileIds(db, entityId, entityType);
            return (from pr in db.PullRequests
                where profileIds.Contains(pr.AuthorProfileId.Value)
                where pr.Created >= period.From && pr.Created < period.To
                select new RawMetric
                {
                    Author = new TableData.Profile($"{pr.AuthorProfile.FirstName} {pr.AuthorProfile.LastName}",
                        pr.AuthorProfile.Username),
                    PullRequest = new TableData.ExternalLink(pr.Title, pr.ExternalUrl),
                    Date = new TableData.DateInSeconds(pr.Created)

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