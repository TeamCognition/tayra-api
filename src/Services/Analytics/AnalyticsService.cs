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

        public MetricDto[] GetAnalyticsWithBreakdown(int profileId, int fromId, int toId)
        {
            var metrics = (from m in DbContext.ProfileMetrics
                where m.DateId >= fromId && m.DateId <= toId
                where m.ProfileId == profileId
                select new MetricRaw
                {
                    Type = m.Type,
                    Value = m.Value,
                    DateId = m.DateId
                }).ToArray();

            return MetricType.List.Select(type => new MetricDto(type, new DatePeriod(fromId, toId), metrics)).ToArray();
        }

        #endregion
    }
}
