using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class CommitsMetric : Metric
    {
        public CommitsMetric(IEnumerable<GitCommit> commits, int dateId) : base(MetricType.Commits, dateId)
        {
            Value = commits.Count();
        }
    }
}