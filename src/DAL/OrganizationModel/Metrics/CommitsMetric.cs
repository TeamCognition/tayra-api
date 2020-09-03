using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class CommitsMetric : Metric
    {
        public CommitsMetric(IEnumerable<GitCommit> commits, int dateId) : base(MetricType.Commits, dateId)
        {
            Value = commits.Count();
        }
    }
}