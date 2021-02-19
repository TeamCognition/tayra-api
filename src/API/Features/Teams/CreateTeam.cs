using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Teams
{
    public partial class TeamsController
    {
        [HttpPost("create")]
        public async Task<Unit> CreateTeam([FromBody] CreateTeam.Command command)
            => await _mediator.Send(command);
    }

    public class CreateTeam
    {
        public record Command : IRequest
        {
            public Guid SegmentId { get; init; }
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
                _db.Add(new Team
                {
                    SegmentId = msg.SegmentId,
                    Key = msg.Key.Trim(),
                    Name = msg.Name.Trim(),
                    AvatarColor = msg.AvatarColor
                });
                await _db.SaveChangesAsync(token);
            }
        }
    }
}