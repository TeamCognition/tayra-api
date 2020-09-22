using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class InventoryValueChangeMetric : PureMetric
    {
        private InventoryValueChangeMetric(float value, int dateId) : base(MetricType.InventoryValueChange, value, dateId)
        {
            
        }
        public static InventoryValueChangeMetric Create(IEnumerable<ProfileInventoryItem> items, int dateId) => new InventoryValueChangeMetric(items.Sum(x => x.Item.Price), dateId);
    }
}