using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class GiftsSentMetric : Metric
    {
        public GiftsSentMetric(IEnumerable<ItemGift> gifts, int dateId) : base(MetricType.GiftsSent, dateId)
        {
            Value = gifts.Count();
        }
    }
}