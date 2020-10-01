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
    public class TokensEarnedMetric : PureMetric
    {
        public TokensEarnedMetric(string name, int value) : base(name, value)
        {
            
        }
        public MetricShard Create(IEnumerable<TokenTransaction> tokenTransactions, int dateId) => new MetricShard((float)tokenTransactions.Where(x => x.Value > 0).Sum(x => x.Value), dateId, this);
        
        public override object[] GetRawMetrics(OrganizationDbContext db, DatePeriod period, int entityId, EntityTypes entityType)
        {
            var profileIds = GetProfileIds(db, entityId, entityType);
            var companyTokenId = db.Tokens.Where(x => x.Type == TokenType.CompanyToken).Select(x => x.Id).FirstOrDefault();
            if (companyTokenId == 0)
                throw new ApplicationException("COMPANY TOKEN NOT FOUND");
            
            return (from tt in db.TokenTransactions
                where profileIds.Contains(tt.ProfileId)
                where tt.Value > 0
                where tt.TokenId == companyTokenId
                where tt.DateId >= period.FromId && tt.DateId <= period.ToId
                select new RawMetric
                {
                    Profile = new TableData.Profile($"{tt.Profile.FirstName} {tt.Profile.LastName}",
                        tt.Profile.Username),
                    Value = (float)tt.Value,
                    Date = tt.Created
                }).ToArray<object>();
        }
        
        public class RawMetric
        {
            public TableData.Profile Profile { get; set; }
            public float Value { get; set; }
            public DateTime Date { get; set; }
        }
    }
}