using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class InventoryValueChangeMetric : Metric
    {
        public InventoryValueChangeMetric(IEnumerable<ProfileInventoryItem> items, int dateId) : base(MetricType.InventoryValueChange, dateId)
        {
            Value = items.Sum(x => x.Item.Price);
        }
    }
}