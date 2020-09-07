using System;
using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class TokensSpentMetric : Metric
    {
        public TokensSpentMetric(IEnumerable<TokenTransaction> tokenTransactions, int dateId) : base(MetricType.TokensSpent, dateId)
        {
            Value = (float)Math.Abs(tokenTransactions.Where(x => x.Value < 0).Sum(x => x.Value));
        }
    }
}