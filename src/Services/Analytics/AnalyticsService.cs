using System;
using System.Linq;
using Cog.Core;
using Cog.DAL;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class AnalyticsService : BaseService<OrganizationDbContext>, IAnalyticsService
    {
        #region Constructor

        public AnalyticsService(OrganizationDbContext dbContext) : base(dbContext)
        {
           
        }

        #endregion

        #region Public Methods

        public AnalyticsMetricDto[] GetAnalyticsWithBreakdown(int entityId, string entityType, DatePeriod period)
        {
            var metrics = (from m in DbContext.ProfileMetrics
                where m.DateId >= period.FromId && m.DateId <= period.ToId
                where m.ProfileId == entityId
                select new MetricRaw
                {
                    Type = m.Type,
                    Value = m.Value,
                    DateId = m.DateId
                }).ToArray();

            return MetricType.List.Select(type => new AnalyticsMetricDto(type, period, metrics)).ToArray();
        }

        #endregion
    }
}
