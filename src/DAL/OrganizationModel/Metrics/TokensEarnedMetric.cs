using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class TokensEarnedMetric : Metric
    {
        public TokensEarnedMetric(IEnumerable<TokenTransaction> tokenTransactions) : base(MetricTypes.TokensEarned)
        {
            Value = (float) tokenTransactions.Where(x => x.Value > 0).Sum(x => x.Value);
        }
    }
}