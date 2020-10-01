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
        
        public override object[] GetRawMetrics(OrganizationDbContext db, DatePeriod period, int entityId, EntityTypes entityType)
        {
            var profileIds = GetProfileIds(db, entityId, entityType);
            return (from p in db.ProfilePraises
                where profileIds.Contains(p.PraiserProfileId)
                where p.DateId >= period.FromId && p.DateId <= period.ToId
                select new RawMetric
                {
                    Praised = new TableData.Profile($"{p.Profile.FirstName} {p.Profile.LastName}",
                        p.Profile.Username),
                    Type = p.Type,
                    Date = p.Created,
                    Message = p.Message
                }).ToArray<object>();
        }

        public class RawMetric
        {
            public TableData.Profile Praised { get; set; }
            public PraiseTypes Type { get; set; }
            public DateTime Date { get; set; }
            public string Message { get; set; }
        }
    }
}