using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class ItemsDisenchantedMetric : PureMetric
    {
        private ItemsDisenchantedMetric(float value, int dateId) : base(MetricType.ItemsDisenchanted, value, dateId)
        {
            
        }
        public static ItemsDisenchantedMetric Create(IEnumerable<ItemDisenchant> disenchants, int dateId) => new ItemsDisenchantedMetric(disenchants.Count(), dateId);
    }
}