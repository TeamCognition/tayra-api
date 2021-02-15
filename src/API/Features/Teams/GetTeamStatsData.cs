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

namespace Tayra.API.Features.Teams
{
    public partial class TeamsController
    {
        [HttpGet("statsWidget/{teamId}")]
        public async Task<GetTeamStatsData.Result> GetTeamStatsData([FromUri] Guid teamId)
        {
            return await _mediator.Send(new GetTeamStatsData.Query {TeamId = teamId});
        }
    }

    public class GetTeamStatsData
    {
        public record Query : IRequest<Result>
        {
            public Guid TeamId { get; init; }
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

                var teamMetrics = metricService.GetMetricsWithIterationSplit(
                    metricList, msg.TeamId, EntityTypes.Team,
                    new DatePeriod(DateTime.UtcNow.AddDays(-27), DateTime.UtcNow));


                var teamsSegmentId = _db.Teams.Where(x => x.Id == msg.TeamId)
                    .Select(x => x.SegmentId).FirstOrDefault();

                var segmentMetrics = metricService.GetMetricsWithIterationSplit(
                    metricList, teamsSegmentId, EntityTypes.Segment,
                    new DatePeriod(DateTime.UtcNow.AddDays(-27), DateTime.UtcNow));

                await Task.Delay(1, token);
                return new Result
                {
                    LastRefreshAt = _db.TeamMetrics.OrderByDescending(x => x.DateId).Select(x => x.Created)
                        .FirstOrDefault(),
                    EntityMetrics = teamMetrics,
                    ComparatorMetrics = segmentMetrics
                };
            }
        }
    }
}