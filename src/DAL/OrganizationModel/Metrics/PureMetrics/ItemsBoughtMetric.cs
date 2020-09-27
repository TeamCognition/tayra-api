using System.Collections.Generic;
using System.Linq;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class ItemsBoughtMetric : PureMetric
    {
        public ItemsBoughtMetric(string name, int value) : base(name, value)
        {
            
        }
        public MetricShard Create(IEnumerable<ShopPurchase> purchases, int dateId) => new MetricShard(purchases.Count(), dateId, this);
    }
}