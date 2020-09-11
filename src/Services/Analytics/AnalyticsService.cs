using System.Collections.Generic;
using System.Linq;
using Cog.Core;
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

        public Dictionary<int, AnalyticsMetricDto> GetMetrics(MetricType[] metricsTypes, int entityId, EntityTypes entityType, DatePeriod period)
        {
            var rawMetrics = metricsTypes.Concat(metricsTypes.SelectMany(x => x.BuildingMetrics)).ToArray();
            rawMetrics = rawMetrics.Concat(rawMetrics.SelectMany(x => x.BuildingMetrics)).ToArray();

            MetricRaw[] metrics = null;

            switch (entityType)
            {
                case EntityTypes.Segment:
                    metrics = (from m in DbContext.ProfileMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where m.ProfileId == entityId
                        where rawMetrics.Contains(m.Type)
                        select new MetricRaw
                        {
                            Type = m.Type,
                            Value = m.Value,
                            DateId = m.DateId
                        }).ToArray();
                    break;
                default:
                    metrics = (from m in DbContext.ProfileMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where m.ProfileId == entityId
                        where rawMetrics.Contains(m.Type)
                        select new MetricRaw
                        {
                            Type = m.Type,
                            Value = m.Value,
                            DateId = m.DateId
                        }).ToArray();
                    break;
            }
            
            return metricsTypes.ToDictionary(type => type.Value,
                type => new AnalyticsMetricDto(type, period, metrics));
        }
        
        
        public Dictionary<int, AnalyticsMetricWithIterationSplitDto> GetMetricsWithIterationSplit(MetricType[] metricsTypes, int entityId, EntityTypes entityType, DatePeriod period)
        {
            var rawMetrics = metricsTypes.Concat(metricsTypes.SelectMany(x => x.BuildingMetrics)).ToArray();
            rawMetrics = rawMetrics.Concat(rawMetrics.SelectMany(x => x.BuildingMetrics)).ToArray();

            MetricRaw[] metrics = null;

            switch (entityType)
            {
                case EntityTypes.Segment:
                    metrics = (from m in DbContext.ProfileMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where m.ProfileId == entityId
                        where rawMetrics.Contains(m.Type)
                        select new MetricRaw
                        {
                            Type = m.Type,
                            Value = m.Value,
                            DateId = m.DateId
                        }).ToArray();
                    break;
                default:
                    metrics = (from m in DbContext.ProfileMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where m.ProfileId == entityId
                        where rawMetrics.Contains(m.Type)
                        select new MetricRaw
                        {
                            Type = m.Type,
                            Value = m.Value,
                            DateId = m.DateId
                        }).ToArray();
                    break;
            }
            
            return metricsTypes.ToDictionary(type => type.Value,
                type => new AnalyticsMetricWithIterationSplitDto(type, period, metrics));
        }
        
        
        public Dictionary<int, AnalyticsMetricWithBreakdownDto> GetMetricsWithBreakdown(int entityId, EntityTypes entityType, DatePeriod period)
        {
            var metricList = new[]
            {
                MetricType.Impact, MetricType.Speed, MetricType.Commits, MetricType.CommitRate,
                MetricType.TasksCompleted, MetricType.Complexity, MetricType.Power, MetricType.TimeWorkedLogged, MetricType.Heat,
                MetricType.Assists, MetricType.PraisesGiven, MetricType.TokensEarned, MetricType.TokensSpent, MetricType.GiftsReceived, MetricType.GiftsSent 
            };

            MetricRaw[] metrics = null;

            switch (entityType)
            {
                case EntityTypes.Segment:
                    metrics = (from m in DbContext.ProfileMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where m.ProfileId == entityId
                        select new MetricRaw
                        {
                            Type = m.Type,
                            Value = m.Value,
                            DateId = m.DateId
                        }).ToArray();
                    break;
                default:
                    metrics = (from m in DbContext.ProfileMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where m.ProfileId == entityId
                        select new MetricRaw
                        {
                            Type = m.Type,
                            Value = m.Value,
                            DateId = m.DateId
                        }).ToArray();
                    break;
            }

            var lastUpdatedAt = DbContext.ProfileMetrics.OrderByDescending(x => x.DateId).Select(x => x.Created)
                .FirstOrDefault();
            
            return metricList.ToDictionary(type => type.Value,
                type => new AnalyticsMetricWithBreakdownDto(type, period, metrics, lastUpdatedAt));
        }

        #endregion
    }
}