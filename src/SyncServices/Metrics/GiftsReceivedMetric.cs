using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class GiftsReceivedMetric : Metric
    {
        public GiftsReceivedMetric(IEnumerable<ItemGift> gifts, int dateId) : base(MetricType.GiftsReceived, dateId)
        {
            Value = gifts.Count();
        }
    }
}