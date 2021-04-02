using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cog.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tayra.Analytics;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Models.Organizations.Metrics;
using Tayra.Services;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Segments
{
    public partial class SegmentsController
    {
        [HttpGet("{segmentKey}/rankChart")]
        public async Task<GetSegmentRankChart.Result> GetSegmentRankChart([FromRoute] string segmentKey) =>
            await _mediator.Send(new GetSegmentRankChart.Query {SegmentKey = segmentKey});
    }

    public class GetSegmentRankChart
    {
        public record Query : IRequest<Result>
        {
            public string SegmentKey { get; init; }
        }

        public record Result
        {
            public Dictionary<int, MetricsValueWEntity[]> EntityMetrics { get; set; }
        }


        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var metricService = new MetricService(_db);

                var metricList = new[]
                {
                    MetricType.Impact, MetricType.Speed, MetricType.Power, MetricType.Assists, MetricType.Heat,
                    MetricType.TasksCompleted, MetricType.Complexity
                };

                var segmentProfiles = _db.ProfileAssignments
                    .Where(x => x.Segment.Key == msg.SegmentKey && x.Profile.IsAnalyticsEnabled)
                    .Select(x => x.ProfileId)
                    .Distinct().ToArray();

                await Task.Delay(1, token);
                return new Result
                {
                    EntityMetrics = metricService.GetMetricsRanks(
                        metricList, segmentProfiles, EntityTypes.Profile,
                        new DatePeriod(DateTime.UtcNow.AddDays(-27), DateTime.UtcNow)
                    )
                };
            }
        }
    }
}