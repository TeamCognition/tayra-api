using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class TokensEarnedMetric : Metric
    {
        public TokensEarnedMetric(IEnumerable<TokenTransaction> tokenTransactions, int dateId) : base(MetricType.TokensEarned, dateId)
        {
            Value = (float)tokenTransactions.Where(x => x.Value > 0).Sum(x => x.Value);
        }
    }
}