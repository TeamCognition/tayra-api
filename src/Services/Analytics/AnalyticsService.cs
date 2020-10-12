using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
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

        public Dictionary<int, MetricValue> GetMetrics(MetricType[] metricsTypes, int entityId, EntityTypes entityType, DatePeriod period)
        {
            var rawMetrics = metricsTypes.Concat(metricsTypes.SelectMany(x => x.BuildingMetrics)).ToArray();
            rawMetrics = rawMetrics.Concat(rawMetrics.SelectMany(x => x.BuildingMetrics)).ToArray();

            MetricShard[] metrics = null;

            switch (entityType)
            {
                case EntityTypes.Segment:
                    metrics = (from m in DbContext.SegmentMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where m.SegmentId == entityId
                        where rawMetrics.Contains(m.Type)
                        select new MetricShard
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
                        select new MetricShard
                        {
                            Type = m.Type,
                            Value = m.Value,
                            DateId = m.DateId
                        }).ToArray();
                    break;
            }
            
            return metricsTypes.ToDictionary(type => type.Value,
                type => new MetricValue(type, period, metrics, entityType));
        }
        
        
        public Dictionary<int, AnalyticsMetricWithIterationSplitDto> GetMetricsWithIterationSplit(MetricType[] metricTypes, int entityId, EntityTypes entityType, DatePeriod period)
        {
            var rawMetrics = metricTypes.Concat(metricTypes.SelectMany(x => x.BuildingMetrics)).ToArray();
            rawMetrics = rawMetrics.Concat(rawMetrics.SelectMany(x => x.BuildingMetrics)).ToArray();

            MetricShard[] metrics = null;
            
            switch (entityType)
            {
                case EntityTypes.Segment:
                    metrics = (from m in DbContext.SegmentMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where m.SegmentId == entityId
                        where rawMetrics.Contains(m.Type)
                        select new MetricShard
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
                        select new MetricShard
                        {
                            Type = m.Type,
                            Value = m.Value,
                            DateId = m.DateId
                        }).ToArray();
                    break;
            }
            
            return metricTypes.ToDictionary(type => type.Value,
                type => new AnalyticsMetricWithIterationSplitDto(type, period, metrics, entityType));
        }

        public Dictionary<int, AnalyticsMetricWithBreakdownDto> GetMetricsWithBreakdown(MetricType[] metricTypes, int entityId, EntityTypes entityType, DatePeriod period)
        {
            if(metricTypes.Length == 0)
                metricTypes = new MetricType[]
                {
                    MetricType.Impact, MetricType.Speed, MetricType.Commits, MetricType.CommitRate,
                    MetricType.TasksCompleted, MetricType.Complexity, MetricType.Power, MetricType.TimeWorkedLogged, MetricType.Heat,
                    MetricType.Assists, MetricType.PraisesGiven, MetricType.TokensEarned, MetricType.TokensSpent, MetricType.GiftsReceived, MetricType.GiftsSent 
                };

            MetricShard[] metrics = null;
            
            switch (entityType)
            {
                case EntityTypes.Segment:
                    metrics = (from m in DbContext.SegmentMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where m.SegmentId == entityId
                        select new MetricShard
                        {
                            Type = m.Type,
                            Value = m.Value,
                            DateId = m.DateId
                        }).ToArray();
                    break;
                case EntityTypes.Team:
                    metrics = (from m in DbContext.TeamMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where m.TeamId == entityId
                        select new MetricShard
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
                        select new MetricShard
                        {
                            Type = m.Type,
                            Value = m.Value,
                            DateId = m.DateId
                        }).ToArray();
                    break;
            }

            var lastRefreshAt = DbContext.ProfileMetrics.OrderByDescending(x => x.DateId).Select(x => x.Created)
                .FirstOrDefault();
            
            return metricTypes.ToDictionary(type => type.Value,
                type => new AnalyticsMetricWithBreakdownDto(type, period, metrics, lastRefreshAt, entityType));
        }

        
        public Dictionary<int, MetricsValueWEntity[]> GetMetricsRanks(MetricType[] metricTypes, int[] entityIds, EntityTypes entityType, DatePeriod period)
        {
            MetricShardWEntity[] metrics = null;
        
            switch (entityType)
            {
                case EntityTypes.Segment:
                    metrics = (from m in DbContext.SegmentMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where entityIds.Contains(m.SegmentId)
                        select new MetricShardWEntity
                        {
                            EntityId = m.SegmentId,
                            Type = m.Type, 
                            Value = m.Value,
                            DateId = m.DateId
                        }).ToArray();
                    break;
                case EntityTypes.Team:
                    metrics = (from m in DbContext.TeamMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where entityIds.Contains(m.TeamId)
                        select new MetricShardWEntity
                        {
                            EntityId = m.TeamId,
                            Type = m.Type,
                            Value = m.Value,
                            DateId = m.DateId
                        }).ToArray();
                    break;
                default:
                    metrics = (from m in DbContext.ProfileMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where entityIds.Contains(m.ProfileId)
                        select new MetricShardWEntity
                        {
                            EntityId = m.ProfileId,
                            Type = m.Type,
                            Value = m.Value,
                            DateId = m.DateId
                        }).ToArray();
                    break;
            }

            return metricTypes.ToDictionary(type => type.Value,
                type => entityIds.Select(eId => MetricsValueWEntity.Create(type, eId, period, metrics, entityType)).OrderByDescending(x => x.Value).ToArray());
        }

        #endregion
    }
}