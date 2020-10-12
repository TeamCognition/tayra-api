using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class ItemsDisenchantedMetric : PureMetric
    {
        public ItemsDisenchantedMetric(string name, int value) : base(name, value)
        {
            
        }
        public MetricShard Create(IEnumerable<ItemDisenchant> disenchants, int dateId) => new MetricShard(disenchants.Count(), dateId, this);
        
        public override object[] GetRawMetrics(OrganizationDbContext db, DatePeriod period, int entityId, EntityTypes entityType)
        {
            var profileIds = GetProfileIds(db, entityId, entityType);
            return (from i in db.ItemDisenchants
                where profileIds.Contains(i.ProfileId)
                where i.DateId >= period.FromId && i.DateId <= period.ToId
                select new RawMetric
                {
                    Profile = new TableData.Profile($"{i.Profile.FirstName} {i.Profile.LastName}",
                        i.Profile.Username),
                    Item = i.Item.Name,
                    Price = i.Item.Price,
                    DisenchantedAt = new TableData.DateInSeconds(i.Created)
                }).ToArray<object>();
        }
        
        public override Type TypeOfRawMetric => typeof(RawMetric);
        
        public class RawMetric
        {
            public TableData.Profile Profile { get; set; }
            public string Item { get; set; }
            public float Price { get; set; }
            public TableData.DateInSeconds DisenchantedAt { get; set; }
        }
    }
}