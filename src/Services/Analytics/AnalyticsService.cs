using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Cog.Core;
using MoreLinq.Extensions;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services.Analytics
{
    public class AnalyticsService : BaseService<OrganizationDbContext>, IAnalyticsService
    {

        #region Constructor

        public AnalyticsService(OrganizationDbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        public Dictionary<int, AnalyticsMetricDto> GetMetrics(List<MetricType> metricsTypes, int entityId, string entityType, DatePeriod period)
        {
            var rawMetrics = metricsTypes.Concat(metricsTypes.SelectMany(x => x.BuildingMetrics));
            
            var metrics = (from m in DbContext.ProfileMetrics
                where m.DateId >= period.FromId && m.DateId <= period.ToId
                where m.ProfileId == entityId
                where rawMetrics.Contains(m.Type)
                select new MetricRaw
                {
                    Type = m.Type,
                    Value = m.Value,
                    DateId = m.DateId
                }).ToArray();
            
            return metricsTypes.ToDictionary(type => type.Value,
                type => new AnalyticsMetricDto(type, period, metrics));
        }
        
        public Dictionary<int, AnalyticsMetricDto> GetAnalyticsWithBreakdown(int entityId, string entityType, DatePeriod period)
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
            
            return MetricType.List.ToDictionary(type => type.Value,
                type => new AnalyticsMetricDto(type, period, metrics));
        }

        #endregion
    }
}