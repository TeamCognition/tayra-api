using System;
using System.Linq;
using Cog.Core;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class PraisesReceivedMetric : PureMetric
    {
        public PraisesReceivedMetric(string name, int value) : base(name, value)
        {
        }
        
        public MetricShard Create(ProfilePraise[] praises, int profileId, int dateId) => new MetricShard(praises.Count(x => x.ProfileId == profileId), dateId, this);

        public RawMetric[] GetRawMetrics(OrganizationDbContext db, int profileId, DatePeriod period)
        {
            return (from p in db.ProfilePraises
                where profileId == p.ProfileId
                where p.DateId >= period.FromId && p.DateId <= period.ToId
                join praiser in db.Profiles on p.PraiserProfileId equals praiser.Id
                select new RawMetric
                {
                    Praiser = new TableDataProfile($"{praiser.FirstName} {praiser.LastName}",
                        praiser.Username),
                    Type = p.Type,
                    Date = p.Created,
                    Message = p.Message
                }).ToArray();
        }

        public class RawMetric
        {
            public TableDataProfile Praiser { get; set; }
            public PraiseTypes Type { get; set; }
            public DateTime Date { get; set; }
            public string Message { get; set; }
        }
    }
}