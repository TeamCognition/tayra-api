using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ItemsDisenchantedMetric : Metric
    {
        public ItemsDisenchantedMetric(IEnumerable<ItemDisenchant> disenchants, int dateId): base(MetricTypes.ItemsDisenchanted, dateId)
        {
            Value = disenchants.Count();
        }
    }
}