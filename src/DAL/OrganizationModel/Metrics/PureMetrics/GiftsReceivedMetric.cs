using System.Collections.Generic;
using System.Linq;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class GiftsReceivedMetric : PureMetric
    {
        public GiftsReceivedMetric(string name, int value) : base(name, value)
        {
            
        }
        public MetricShard Create(IEnumerable<ItemGift> gifts, int dateId) => new MetricShard(gifts.Count(), dateId, this);
    }
}