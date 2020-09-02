using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class GiftsSentMetric : Metric
    {
        public GiftsSentMetric(IEnumerable<ItemGift> gifts, int dateId): base(MetricType.GiftsSent, dateId)
        {
            Value = gifts.Count();
        }
    }
}