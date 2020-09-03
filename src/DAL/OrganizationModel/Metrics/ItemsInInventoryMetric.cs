using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ItemsInInventoryMetric : Metric
    {
        public ItemsInInventoryMetric(IEnumerable<ProfileInventoryItem> items, int dateId) : base(MetricType.ItemsInInventory, dateId)
        {
            Value = items.Count();
        }
    }
}