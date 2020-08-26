using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ItemsBoughtMetric : Metric
    {
        public ItemsBoughtMetric(IEnumerable<ShopPurchase> purchases, int dateId): base(MetricTypes.ItemsBought, dateId)
        {
            Value = purchases.Count();
        }
    }
}