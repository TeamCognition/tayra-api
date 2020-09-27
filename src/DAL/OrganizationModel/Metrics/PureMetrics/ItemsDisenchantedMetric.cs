using System.Collections.Generic;
using System.Linq;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class ItemsDisenchantedMetric : PureMetric
    {
        public ItemsDisenchantedMetric(string name, int value) : base(name, value)
        {
            
        }
        public MetricShard Create(IEnumerable<ItemDisenchant> disenchants, int dateId) => new MetricShard(disenchants.Count(), dateId, this);
    }
}