using System;
using System.Linq;
using Cog.Core;
using Tayra.Analytics;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Analytics.Metrics;
using Tayra.Services;

namespace Tayra.SyncServices.Metrics
{
    public class PraisesGivenMetric : PureMetric
    {
        public PraisesGivenMetric(string name, int value) : base(name, value)
        {
            
        }
        public MetricShard Create(ProfilePraise[] praises, int profileId, int dateId) => new MetricShard(praises.Count(x => x.PraiserProfileId == profileId), dateId, this);
        
        public RawMetric[] GetRawMetrics(OrganizationDbContext db, int profileId, DatePeriod period)
        {
            return (from p in db.ProfilePraises
                where profileId == p.PraiserProfileId
                where p.DateId >= period.FromId && p.DateId <= period.ToId
                select new RawMetric
                {
                    Praised = new TableDataProfile($"{p.Profile.FirstName} {p.Profile.LastName}",
                        p.Profile.Username),
                    Type = p.Type,
                    Date = p.Created,
                    Message = p.Message
                }).ToArray();
        }

        public class RawMetric
        {
            public TableDataProfile Praised { get; set; }
            public PraiseTypes Type { get; set; }
            public DateTime Date { get; set; }
            public string Message { get; set; }
        }
    }
}