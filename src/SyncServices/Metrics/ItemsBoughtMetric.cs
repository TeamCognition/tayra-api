using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class ItemsBoughtMetric : PureMetric
    {
        private ItemsBoughtMetric(float value, int dateId) : base(MetricType.ItemsBought, value, dateId)
        {
            
        }
        public static ItemsBoughtMetric Create(IEnumerable<ShopPurchase> purchases, int dateId) => new ItemsBoughtMetric(purchases.Count(), dateId);
    }
}