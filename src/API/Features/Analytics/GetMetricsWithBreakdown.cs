using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cog.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Analytics;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Models.Organizations.Metrics;
using Result = System.Collections.Generic.Dictionary<int, Tayra.Models.Organizations.Metrics.MetricService.AnalyticsMetricWithBreakdownDto>;

namespace Tayra.API.Features.Analytics
{
    public partial class AnalyticsController
    {
        [HttpGet("metricsWithBreakdown")]
        public async Task<Result> GetAnalyticsWithBreakdown([FromQuery] int[] m, [FromQuery]string period, [FromQuery] GetMetricsWithBreakdown.Query query)
        {
            var metricTypes = m.Select(MetricType.FromValue).ToArray();
            var datePeriod = new DatePeriod(period);
            
            return await _mediator.Send(query with {MetricTypes = metricTypes, Period = datePeriod});
        }
    }
    
    public class GetMetricsWithBreakdown
    {
        public record Query : IRequest<Result>
        {
            public MetricType[] MetricTypes { get; init; }
            public Guid EntityId { get; init; }
            public EntityTypes EntityType { get; init; }
            public DatePeriod Period { get; init; }
        }
        
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var metricTypesToUse = msg.MetricTypes;
                
                if (metricTypesToUse.Length == 0)
                    metricTypesToUse = new []
                {
                    MetricType.Impact, MetricType.Speed, MetricType.Commits, MetricType.CommitRate,
                    MetricType.TasksCompleted, MetricType.Complexity, MetricType.Power, MetricType.TimeWorkedLogged, MetricType.Heat,
                    MetricType.Assists, MetricType.PraisesGiven, MetricType.TokensEarned, MetricType.TokensSpent, MetricType.GiftsReceived, MetricType.GiftsSent
                };

                MetricShard[] metrics = null;

                switch (msg.EntityType)
                {
                    case EntityTypes.Segment:
                        metrics = (from m in _db.SegmentMetrics
                            where m.DateId >= msg.Period.FromId && m.DateId <= msg.Period.ToId
                            where m.SegmentId == msg.EntityId
                            select new MetricShard
                            {
                                Type = m.Type,
                                Value = m.Value,
                                DateId = m.DateId
                            }).ToArray();
                        break;
                    case EntityTypes.Team:
                        metrics = (from m in _db.TeamMetrics
                            where m.DateId >= msg.Period.FromId && m.DateId <= msg.Period.ToId
                            where m.TeamId == msg.EntityId
                            select new MetricShard
                            {
                                Type = m.Type,
                                Value = m.Value,
                                DateId = m.DateId
                            }).ToArray();
                        break;
                    default:
                        metrics = (from m in _db.ProfileMetrics
                            where m.DateId >= msg.Period.FromId && m.DateId <= msg.Period.ToId
                            where m.ProfileId == msg.EntityId
                            select new MetricShard
                            {
                                Type = m.Type,
                                Value = m.Value,
                                DateId = m.DateId
                            }).ToArray();
                        break;
                }

                var lastRefreshAt = await _db.ProfileMetrics.OrderByDescending(x => x.DateId).Select(x => x.Created)
                    .FirstOrDefaultAsync(token);

                return metricTypesToUse.ToDictionary(type => type.Value,
                    type => new MetricService.AnalyticsMetricWithBreakdownDto(type, msg.Period, metrics, lastRefreshAt, msg.EntityType));
            }
        }
    }
}