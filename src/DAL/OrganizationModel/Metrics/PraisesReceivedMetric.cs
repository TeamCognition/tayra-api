using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class PraisesReceivedMetric : Metric
    {
        public PraisesReceivedMetric(ProfilePraise[] praises, int profileId, int dateId) : base(type: MetricTypes.PraisesReceived, dateId)
        {
            Value = praises.Count(x => x.ProfileId == profileId);
        }
    }
}