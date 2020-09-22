using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class TokensEarnedMetric : PureMetric
    {
        private TokensEarnedMetric(float value, int dateId) : base(MetricType.TokensEarned, value, dateId)
        {
            
        }
        public static TokensEarnedMetric Create(IEnumerable<TokenTransaction> tokenTransactions, int dateId) => new TokensEarnedMetric((float)tokenTransactions.Where(x => x.Value > 0).Sum(x => x.Value), dateId);
    }
}