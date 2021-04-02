using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cog.Core;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Analytics;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Models.Organizations.Metrics;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Segments
{
    public partial class SegmentsController
    {
        [HttpGet("{segmentKey}/averageMetrics")]
        public async Task<GetSegmentAverageMetrics.Result> GetSegmentAverageMetrics([FromRoute] string segmentKey) =>
            await _mediator.Send(new GetSegmentAverageMetrics.Query {SegmentKey = segmentKey});
    }

    public class GetSegmentAverageMetrics
    {
        public record Query : IRequest<Result>
        {
            public string SegmentKey { get; init; }
        }

        public record Result
        {
            public Dictionary<int, MetricService.AnalyticsMetricWithIterationSplitDto> EntityMetrics { get; set; }
        }


        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var segment = await _db.Segments.Where(x => x.Key == msg.SegmentKey).FirstOrDefaultAsync(token);
                segment.EnsureNotNull(msg.SegmentKey);

                var metricService = new MetricService(_db);
                var metricList = new[]
                {
                    MetricType.Impact, MetricType.Speed, MetricType.Power, MetricType.Assists, MetricType.Heat,
                    MetricType.TasksCompleted, MetricType.Complexity
                };

                await Task.Delay(1, token);
                return new Result
                {
                    EntityMetrics = metricService.GetMetricsWithIterationSplit(
                        metricList, segment.Id, EntityTypes.Segment,
                        new DatePeriod(DateTime.UtcNow.AddDays(-27), DateTime.UtcNow))
                };
            }
        }
    }
}