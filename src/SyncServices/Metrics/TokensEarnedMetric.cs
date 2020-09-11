using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class TokensEarnedMetric : Metric
    {
        public TokensEarnedMetric(IEnumerable<TokenTransaction> tokenTransactions, int dateId) : base(MetricType.TokensEarned, dateId)
        {
            Value = (float)tokenTransactions.Where(x => x.Value > 0).Sum(x => x.Value);
        }
    }
}