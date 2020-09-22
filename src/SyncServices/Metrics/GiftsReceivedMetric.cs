using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class GiftsReceivedMetric : PureMetric
    {
        private GiftsReceivedMetric(float value, int dateId) : base(MetricType.GiftsReceived, value, dateId)
        {
            
        }
        public static GiftsReceivedMetric Create(IEnumerable<ItemGift> gifts, int dateId) => new GiftsReceivedMetric(gifts.Count(), dateId);
    }
}