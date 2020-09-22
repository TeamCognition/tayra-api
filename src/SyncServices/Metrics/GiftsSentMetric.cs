using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class GiftsSentMetric : PureMetric
    {
        private GiftsSentMetric(float value, int dateId) : base(MetricType.GiftsSent, value, dateId)
        {
            
        }
        public static GiftsSentMetric Create(IEnumerable<ItemGift> gifts, int dateId) => new GiftsSentMetric(gifts.Count(), dateId);
    }
}