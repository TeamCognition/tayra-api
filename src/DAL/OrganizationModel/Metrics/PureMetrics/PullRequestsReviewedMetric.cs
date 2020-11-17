using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class PullRequestsReviewedMetric : PureMetric
    {
        public PullRequestsReviewedMetric(string name, int value) : base(name, value)
        {
            
        }

        public MetricShard Create(IEnumerable<PullRequest> pullRequests, int dateId) =>
            new MetricShard(pullRequests.Count(), dateId, this);

        public override object[] GetRawMetrics(OrganizationDbContext db, DatePeriod period, int entityId, EntityTypes entityType)
        {
            throw new System.NotImplementedException();
        }

        public override Type TypeOfRawMetric => typeof(RawMetric);

        public class RawMetric
        {
            
        }
    }
}