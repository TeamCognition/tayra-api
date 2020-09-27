using System.Collections.Generic;
using System.Linq;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class CommitsMetric : PureMetric
    {
        public CommitsMetric(string name, int value) : base(name, value)
        {
            
        }
        public MetricShard Create(IEnumerable<GitCommit> commits, int dateId) => new MetricShard(commits.Count(), dateId, this);
    }
}