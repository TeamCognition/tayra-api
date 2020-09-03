using System;
using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class TokensSpentMetric : Metric
    {
        public TokensSpentMetric(IEnumerable<TokenTransaction> tokenTransactions, int dateId) : base(MetricType.TokensSpent, dateId)
        {
            Value = (float)Math.Abs(tokenTransactions.Where(x => x.Value < 0).Sum(x => x.Value));
        }
    }
}