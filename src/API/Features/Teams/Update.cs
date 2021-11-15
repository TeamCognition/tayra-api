using System;
using System.Threading;
using System.Threading.Tasks;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Organizations;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Teams
{
    public partial class TeamsController
    {
        [HttpPut("{teamId}/update")]
        public async Task<Unit> UpdateTeam([FromRoute] Guid teamId, [FromBody] Update.Command command)
            => await _mediator.Send(command with {TeamId = teamId});
    }

    public class Update
    {
        public record Command : IRequest
        {
            public Guid TeamId { get; init; }
            public string Key { get; init; }
            public string Name { get; init; }
            public string AvatarColor { get; init; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            protected override async Task Handle(Command msg, CancellationToken token)
            {
                var team = await _db.Teams.FirstOrDefaultAsync(x => x.Id == msg.TeamId, token);

                team.EnsureNotNull(msg.TeamId);

                team.Key = msg.Key.Trim();
                team.Name = msg.Name.Trim();
                team.AvatarColor = msg.AvatarColor;

                await _db.SaveChangesAsync(token);
            }
        }
    }
}