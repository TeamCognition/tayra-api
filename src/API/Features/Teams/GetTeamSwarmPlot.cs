using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Cog.Core;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Analytics;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Models.Organizations.Metrics;
using Tayra.Services;

namespace Tayra.API.Features.Teams
{
    public partial class TeamsController
    {
        [HttpGet("{teamId}/swarmPlot")]
        public async Task<GetTeamSwarmPlot.Result> GetTeamSwarmPlot([FromQuery] Guid teamId)
            => await _mediator.Send(new GetTeamSwarmPlot.Query {TeamId = teamId});
    }

    public class GetTeamSwarmPlot
    {
        public record Query : IRequest<Result>
        {
            public Guid TeamId { get; init; }
        }

        public record Result
        {
            public int LastUpdateDateId { get; set; }
            public Dictionary<int, MetricsValueWEntity[]> ProfileMetrics { get; set; }
            public Dictionary<int, MetricValue> Averages { get; set; }

        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var team = await _db.Teams.FirstOrDefaultAsync(x => x.Id == msg.TeamId);
                team.EnsureNotNull(msg.TeamId);

                var metricService = new MetricService(_db);

                var metricList = new []
                {
                    MetricType.Impact, MetricType.Speed, MetricType.Power, MetricType.Assists, MetricType.Heat,
                    MetricType.TasksCompleted, MetricType.Complexity
                };

                var teamProfiles = await _db.ProfileAssignments.Where(x => x.TeamId == team.Id && x.Profile.IsAnalyticsEnabled).Select(x => x.ProfileId)
                    .ToArrayAsync(token);

                var lastUpdateDateId = await _db.TeamMetrics.OrderByDescending(x => x.DateId).Select(x => x.DateId).FirstOrDefaultAsync(token);
                
                return new Result
                {
                    LastUpdateDateId = lastUpdateDateId,

                    ProfileMetrics = metricService.GetMetricsRanks(metricList, teamProfiles, EntityTypes.Profile,
                        new DatePeriod(DateTime.UtcNow.AddDays(-27), DateTime.UtcNow)),
                    
                    Averages = metricService.GetMetrics(metricList, team.Id, EntityTypes.Team,
                        new DatePeriod(DateTime.UtcNow.AddDays(-27), DateTime.UtcNow))
                };
            }
        }
    }
}