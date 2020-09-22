using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class CommitsMetric : PureMetric
    {
        private CommitsMetric(float value, int dateId) : base(MetricType.Commits, value, dateId)
        {
            
        }
        public static CommitsMetric Create(IEnumerable<GitCommit> commits, int dateId) => new CommitsMetric(commits.Count(), dateId);
    }
}