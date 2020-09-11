using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class PraisesGivenMetric : Metric
    {
        public PraisesGivenMetric(ProfilePraise[] praises, int profileId, int dateId) : base(MetricType.PraisesGiven, dateId)
        {
            Value = praises.Count(x => x.PraiserProfileId == profileId);
        }
    }
}