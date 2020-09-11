using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class ItemsBoughtMetric : Metric
    {
        public ItemsBoughtMetric(IEnumerable<ShopPurchase> purchases, int dateId) : base(MetricType.ItemsBought, dateId)
        {
            Value = purchases.Count();
        }
    }
}