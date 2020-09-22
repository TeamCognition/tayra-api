using System;
using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class TokensSpentMetric : PureMetric
    {
        private TokensSpentMetric(float value, int dateId) : base(MetricType.TokensSpent, value, dateId)
        {
            
        }
        public static TokensSpentMetric Create(IEnumerable<TokenTransaction> tokenTransactions, int dateId) => new TokensSpentMetric((float)Math.Abs(tokenTransactions.Where(x => x.Value < 0).Sum(x => x.Value)), dateId);
    }
}