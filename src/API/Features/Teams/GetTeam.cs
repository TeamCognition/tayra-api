using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Organizations;

namespace Tayra.API.Features.Teams
{
    public partial class TeamsController
    {
        [HttpGet("{segmentKey}/{teamKey}")]
        public async Task<GetTeam.Result> GetTeam([FromQuery] string segmentKey, [FromQuery] string teamKey)
            => await _mediator.Send(new GetTeam.Query {SegmentKey = segmentKey, TeamKey = teamKey});
    }

    public class GetTeam
    {
        public record Query : IRequest<Result>
        {
            public string TeamKey { get; init; }
            public string SegmentKey { get; init; }

        }

        public record Result
        {
            public Guid TeamId { get; init; }
            public string TeamKey { get; init; }
            public string Name { get; init; }
            public string AvatarColor { get; init; }
            public string AssistantSummary { get; init; }
            public DateTime Created { get; init; }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var team = await _db.Teams
                    .Where(x => x.Segment.Key == msg.SegmentKey && x.Key == msg.TeamKey)
                    .Select(x => new Result
                    {
                        TeamId = x.Id,
                        TeamKey = x.Key,
                        Name = x.Name,
                        AvatarColor = x.AvatarColor,
                        AssistantSummary = x.AssistantSummary,
                        Created = x.Created
                    })
                    .FirstOrDefaultAsync(token);
                team.EnsureNotNull(msg.TeamKey);

                return team;
            }
        }
    }
}