using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class ItemsDisenchantedMetric : Metric
    {
        public ItemsDisenchantedMetric(IEnumerable<ItemDisenchant> disenchants, int dateId) : base(MetricType.ItemsDisenchanted, dateId)
        {
            Value = disenchants.Count();
        }
    }
}