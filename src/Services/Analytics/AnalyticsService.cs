using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Cog.Core;
using MailChimp.Net.Models;
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
                    metrics = (from m in DbContext.SegmentMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where m.SegmentId == entityId
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
                type => new AnalyticsMetricDto(type, period, metrics, entityType));
        }
        
        
        public Dictionary<int, AnalyticsMetricWithIterationSplitDto> GetMetricsWithIterationSplit(MetricType[] metricTypes, int entityId, EntityTypes entityType, DatePeriod period)
        {
            var rawMetrics = metricTypes.Concat(metricTypes.SelectMany(x => x.BuildingMetrics)).ToArray();
            rawMetrics = rawMetrics.Concat(rawMetrics.SelectMany(x => x.BuildingMetrics)).ToArray();

            MetricRaw[] metrics = null;
            
            switch (entityType)
            {
                case EntityTypes.Segment:
                    metrics = (from m in DbContext.SegmentMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where m.SegmentId == entityId
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
            
            return metricTypes.ToDictionary(type => type.Value,
                type => new AnalyticsMetricWithIterationSplitDto(type, period, metrics, entityType));
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
                    metrics = (from m in DbContext.SegmentMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where m.SegmentId == entityId
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

            var lastRefreshAt = DbContext.ProfileMetrics.OrderByDescending(x => x.DateId).Select(x => x.Created)
                .FirstOrDefault();
            
            return metricList.ToDictionary(type => type.Value,
                type => new AnalyticsMetricWithBreakdownDto(type, period, metrics, lastRefreshAt, entityType));
        }

        
        public Dictionary<int, AnalyticsMetricsWEntityDto[]> GetMetricsRanks(MetricType[] metricTypes, int[] entityIds, EntityTypes entityType, DatePeriod period)
        {
            MetricRawWEntity[] metrics = null;
        
            switch (entityType)
            {
                case EntityTypes.Segment:
                    metrics = (from m in DbContext.SegmentMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where entityIds.Contains(m.SegmentId)
                        select new MetricRawWEntity
                        {
                            EntityId = m.SegmentId,
                            MetricRaw = new MetricRaw
                            {
                                Type = m.Type,
                                Value = m.Value,
                                DateId = m.DateId
                            }
                        }).ToArray();
                    break;
                default:
                    metrics = (from m in DbContext.ProfileMetrics
                        where m.DateId >= period.FromId && m.DateId <= period.ToId
                        where entityIds.Contains(m.ProfileId)
                        select new MetricRawWEntity
                        {
                            EntityId = m.ProfileId,
                            MetricRaw = new MetricRaw
                            {
                                Type = m.Type,
                                Value = m.Value,
                                DateId = m.DateId
                            }
                        }).ToArray();
                    break;
            }

            return metricTypes.ToDictionary(type => type.Value,
                type => entityIds.Select(eId => new AnalyticsMetricsWEntityDto(type, eId, period, metrics, entityType)).OrderByDescending(x => x.Value).ToArray());
        }
        
        public class TableColumn<T> where T: class
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public T[] Values { get; set; }
            
            public TableColumn(string name, string type, T[] values)
            {
                
            }
        }

        public class TaskTableDTO
        {
            public HyperLink Name { get; set; }
            
        }

        public class HyperLink
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
        
        public void Haris()
        {
            var tasks = DbContext.Tasks.Select(x => new
            {
                Priority = x.Priority
            }).ToArray();

            object oo = tasks;
            
            foreach (PropertyInfo prop in oo.GetType().GetProperties())
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                Console.WriteLine(type.Name);
            }
            
        }

        #endregion
    }
}