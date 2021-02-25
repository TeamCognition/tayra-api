using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Cog.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tayra.Analytics;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Models.Organizations.Metrics;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Segments
{
    public partial class SegmentsController
    {
        [HttpGet("statsWidget/{segmentId}")]
        public async Task<GetSegmentStats.Result> GetSegmentStats([FromUri] Guid segmentId)
        {
            return await _mediator.Send(new GetSegmentStats.Query {SegmentId = segmentId});
        }
    }

    public class GetSegmentStats
    {
        public record Query : IRequest<Result>
        {
            public Guid SegmentId { get; init; }
        }

        public record Result
        {
            public DateTime? LastRefreshAt { get; set; }
            public Dictionary<int, MetricService.AnalyticsMetricWithIterationSplitDto> EntityMetrics { get; set; }
            public Dictionary<int, MetricService.AnalyticsMetricWithIterationSplitDto> ComparatorMetrics { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db)
            {
                _db = db;
            }

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var metricService = new MetricService(_db);

                var metricList = new[]
                {
                    MetricType.Impact, MetricType.Speed, MetricType.Power, MetricType.Assists,
                    MetricType.TasksCompleted, MetricType.Complexity, MetricType.CommitRate
                };
                
                var segmentMetrics = metricService.GetMetricsWithIterationSplit(
                    metricList, msg.SegmentId, EntityTypes.Segment,
                    new DatePeriod(DateTime.UtcNow.AddDays(-27), DateTime.UtcNow));
                
                await Task.Delay(1, token);
                return new Result()
                {
                    LastRefreshAt = _db.ProfileMetrics.OrderByDescending(x => x.DateId).Select(x => x.Created).FirstOrDefault(),
                    EntityMetrics = segmentMetrics,
                    ComparatorMetrics = null
                };
            }
        }
    }
}