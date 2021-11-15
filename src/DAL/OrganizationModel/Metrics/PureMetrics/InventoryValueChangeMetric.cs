using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.EntityFrameworkCore;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Metrics
{
    public class InventoryValueChangeMetric : PureMetric
    {
        public InventoryValueChangeMetric(string name, int value) : base(name, value)
        {

        }
        public MetricShard Create(IEnumerable<ProfileInventoryItem> items, int dateId) => new MetricShard(items.Sum(x => x.Item.Price), dateId, this);

        public override object[] GetRawMetrics(OrganizationDbContext db, DatePeriod period, Guid entityId, EntityTypes entityType)
        {
            var profileIds = GetProfileIds(db, entityId, entityType);
            return (from i in db.ProfileInventoryItems.Include(x => x.Item)
                    where profileIds.Contains(i.ProfileId)
                    where i.Created.Date >= period.From && i.Created.Date <= period.To
                    select new RawMetric
                    {
                        Item = Name
                    }).ToArray<object>();
        }

        public override Type TypeOfRawMetric => typeof(RawMetric);

        public class RawMetric
        {
            public string Item { get; set; }
        }
    }
}