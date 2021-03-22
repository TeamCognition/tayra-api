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
    public class GiftsSentMetric : PureMetric
    {
        public GiftsSentMetric(string name, int value) : base(name, value)
        {

        }
        public MetricShard Create(IEnumerable<ItemGift> gifts, int dateId) => new MetricShard(gifts.Count(), dateId, this);

        public override object[] GetRawMetrics(OrganizationDbContext db, DatePeriod period, Guid entityId, EntityTypes entityType)
        {
            var profileIds = GetProfileIds(db, entityId, entityType);
            return (from i in db.ItemGifts
                    where profileIds.Contains(i.ReceiverId)
                    where i.DateId >= period.FromId && i.DateId <= period.ToId
                    select new RawMetric
                    {
                        Sender = new TableData.Profile($"{i.Sender.FirstName} {i.Sender.LastName}",
                            i.Sender.Username),
                        Receiver = new TableData.Profile($"{i.Receiver.FirstName} {i.Receiver.LastName}",
                            i.Receiver.Username),
                        Item = i.Item.Name
                    }).ToArray<object>();
        }

        public override Type TypeOfRawMetric => typeof(RawMetric);

        public class RawMetric
        {
            public TableData.Profile Sender { get; set; }
            public TableData.Profile Receiver { get; set; }
            public string Item { get; set; }
        }
    }
}