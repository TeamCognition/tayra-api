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

        public MetricShard Create(ProfilePraise[] praises, Guid profileId, int dateId) => new MetricShard(praises.Count(x => x.ProfileId == profileId), dateId, this);

        public override object[] GetRawMetrics(OrganizationDbContext db, DatePeriod period, Guid entityId, EntityTypes entityType)
        {
            var profileIds = GetProfileIds(db, entityId, entityType);
            return (from p in db.ProfilePraises
                    where profileIds.Contains(p.ProfileId)
                    where p.DateId >= period.FromId && p.DateId <= period.ToId
                    join praiser in db.Profiles on p.PraiserProfileId equals praiser.Id
                    select new RawMetric
                    {
                        Praiser = new TableData.Profile($"{praiser.FirstName} {praiser.LastName}",
                            praiser.Username),
                        Type = p.Type,
                        Date = new TableData.DateInSeconds(p.Created),
                        Message = p.Message
                    }).ToArray<object>();
        }

        public override Type TypeOfRawMetric => typeof(RawMetric);

        public class RawMetric
        {
            public TableData.Profile Praiser { get; set; }
            public PraiseTypes Type { get; set; }
            public TableData.DateInSeconds Date { get; set; }
            public string Message { get; set; }
        }
    }
}