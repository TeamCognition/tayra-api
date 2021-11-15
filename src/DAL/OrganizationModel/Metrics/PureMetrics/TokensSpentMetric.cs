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
    public class TokensSpentMetric : PureMetric
    {
        public TokensSpentMetric(string name, int value) : base(name, value)
        {

        }
        public MetricShard Create(IEnumerable<TokenTransaction> tokenTransactions, int dateId) => new MetricShard((float)Math.Abs(tokenTransactions.Where(x => x.Value < 0).Sum(x => x.Value)), dateId, this);

        public override object[] GetRawMetrics(OrganizationDbContext db, DatePeriod period, Guid entityId, EntityTypes entityType)
        {
            var profileIds = GetProfileIds(db, entityId, entityType);
            
            return (from tt in db.TokenTransactions
                    where profileIds.Contains(tt.ProfileId)
                    where tt.Value < 0
                    where tt.TokenType == TokenType.CompanyToken
                    where tt.DateId >= period.FromId && tt.DateId <= period.ToId
                    select new RawMetric
                    {
                        Profile = new TableData.Profile($"{tt.Profile.FirstName} {tt.Profile.LastName}",
                            tt.Profile.Username),
                        Value = (float)tt.Value,
                        Date = new TableData.DateInSeconds(tt.Created)
                    }).ToArray<object>();
        }

        public override Type TypeOfRawMetric => typeof(RawMetric);

        public class RawMetric
        {
            public TableData.Profile Profile { get; set; }
            public float Value { get; set; }
            public TableData.DateInSeconds Date { get; set; }
        }
    }
}