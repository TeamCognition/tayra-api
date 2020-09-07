using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class PraisesReceivedMetric : Metric
    {
        public PraisesReceivedMetric(ProfilePraise[] praises, int profileId, int dateId) : base(MetricType.PraisesReceived, dateId)
        {
            Value = praises.Count(x => x.ProfileId == profileId);
        }
    }
}