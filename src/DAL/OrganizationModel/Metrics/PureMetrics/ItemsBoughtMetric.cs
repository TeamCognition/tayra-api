using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Metrics
{
    public class ItemsBoughtMetric : PureMetric
    {
        public ItemsBoughtMetric(string name, int value) : base(name, value)
        {

        }
        public MetricShard Create(IEnumerable<ShopPurchase> purchases, int dateId) => new MetricShard(purchases.Count(), dateId, this);

        public override object[] GetRawMetrics(OrganizationDbContext db, DatePeriod period, Guid entityId, EntityTypes entityType)
        {
            var profileIds = GetProfileIds(db, entityId, entityType);
            return (from sp in db.ShopPurchases
                    where profileIds.Contains(sp.ProfileId)
                    where sp.Status == ShopPurchaseStatuses.Fulfilled
                    where sp.LastModifiedDateId >= period.FromId && sp.LastModifiedDateId <= period.ToId
                    select new RawMetric
                    {
                        Buyer = new TableData.Profile($"{sp.Profile.FirstName} {sp.Profile.LastName}",
                            sp.Profile.Username),
                        Item = sp.Item.Name,
                        Price = sp.Price
                    }).ToArray<object>();
        }

        public override Type TypeOfRawMetric => typeof(RawMetric);

        public class RawMetric
        {
            public TableData.Profile Buyer { get; set; }
            public string Item { get; set; }
            public float Price { get; set; }
        }
    }
}