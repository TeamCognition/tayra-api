using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class GiftsReceivedMetric : Metric
    {
        public GiftsReceivedMetric(IEnumerable<ItemGift> gifts, int dateId): base(MetricTypes.GiftsReceived, dateId)
        {
            Value = gifts.Count();
        }
    }
}