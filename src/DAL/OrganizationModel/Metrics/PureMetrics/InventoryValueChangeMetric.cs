using System.Collections.Generic;
using System.Linq;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class InventoryValueChangeMetric : PureMetric
    {
        public InventoryValueChangeMetric(string name, int value) : base(name, value)
        {
            
        }
        public MetricShard Create(IEnumerable<ProfileInventoryItem> items, int dateId) => new MetricShard(items.Sum(x => x.Item.Price), dateId, this);
    }
}