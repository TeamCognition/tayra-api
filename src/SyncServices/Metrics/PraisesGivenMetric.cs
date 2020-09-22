using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class PraisesGivenMetric : PureMetric
    {
        private PraisesGivenMetric(float value, int dateId) : base(MetricType.PraisesGiven, value, dateId)
        {
            
        }
        public static PraisesGivenMetric Create(ProfilePraise[] praises, int profileId, int dateId) => new PraisesGivenMetric(praises.Count(x => x.PraiserProfileId == profileId), dateId);
    }
}