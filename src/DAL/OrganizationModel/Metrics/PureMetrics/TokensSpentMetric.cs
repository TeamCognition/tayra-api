using System;
using System.Collections.Generic;
using System.Linq;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class TokensSpentMetric : PureMetric
    {
        public TokensSpentMetric(string name, int value) : base(name, value)
        {
            
        }
        public MetricShard Create(IEnumerable<TokenTransaction> tokenTransactions, int dateId) => new MetricShard((float)Math.Abs(tokenTransactions.Where(x => x.Value < 0).Sum(x => x.Value)), dateId, this);
    }
}