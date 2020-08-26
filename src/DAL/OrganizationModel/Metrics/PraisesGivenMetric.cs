using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class PraisesGivenMetric : Metric
    {
        public PraisesGivenMetric(ProfilePraise[] praises, int profileId, int dateId) : base(MetricTypes.PraisesGiven, dateId)
        {
            Value = praises.Count(x => x.PraiserProfileId == profileId);
        }
    }
}