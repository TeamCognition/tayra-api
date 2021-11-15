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

namespace Tayra.API.Features.Teams
{
    public partial class TeamsController
    {
        [HttpGet("{teamId}/rawScore")]
        public async Task<GetRawScore.Result> GetTeamRawScore([FromQuery] Guid teamId)
            => await _mediator.Send(new GetRawScore.Query {TeamId = teamId});
    }

    public class GetRawScore
    {
        public record Query : IRequest<Result>
        {
            public Guid TeamId { get; init; }
        }

        public record Result
        {
            public Dictionary<int, MetricValue> Metrics { get; init; }
            public int DaysOnTayra { get; init; }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var team = await _db.Teams.FirstOrDefaultAsync(x => x.Id == msg.TeamId, token);
                team.EnsureNotNull(team,msg.TeamId);
                
                var metricTypes = new[] { MetricType.TasksCompleted, MetricType.Assists, MetricType.TimeWorked, MetricType.TokensEarned, MetricType.TokensSpent, MetricType.ItemsBought };
                
                var shards = await (from m in _db.SegmentMetrics
                    where m.SegmentId == msg.TeamId
                    where metricTypes.Contains(m.Type)
                    select new MetricShard
                    {
                        Type = m.Type,
                        Value = m.Value,
                        DateId = m.DateId
                    }).ToArrayAsync(token);

                return new Result
                {
                    Metrics = metricTypes.ToDictionary(type => type.Value,
                        type => new MetricValue(type, new DatePeriod(team.Created, DateTime.UtcNow), shards,
                            EntityTypes.Segment)),
                    DaysOnTayra = (DateTime.UtcNow - team.Created).Days
                };
            }
        }
    }
}