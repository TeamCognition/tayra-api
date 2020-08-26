using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class InventoryValueMetric : Metric
    {
        public InventoryValueMetric(IEnumerable<ProfileInventoryItem> items, int dateId): base(MetricTypes.InventoryValue, dateId)
        {
            Value = items.Sum(x => x.Item.Price);
        }
    }
}