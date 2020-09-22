using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services.Analytics;

namespace Tayra.SyncServices.Metrics
{
    public class PraisesReceivedMetric : PureMetric
    {
        private PraisesReceivedMetric(float value, int dateId) : base(MetricType.PraisesReceived, value, dateId)
        {
            
        }
        public static PraisesReceivedMetric Create(ProfilePraise[] praises, int profileId, int dateId) => new PraisesReceivedMetric(praises.Count(x => x.ProfileId == profileId), dateId);
        
        //public static AnalyticsService.TableData<>

        public class RawMetric
        {
            public PraiserDto Praiser { get; set; }

            public class PraiserDto
            {
                
            }
        }
    }
}