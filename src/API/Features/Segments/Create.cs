using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tayra.Common;
using Tayra.Models.Organizations;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Segments
{
    public partial class SegmentsController
    {
        [HttpPost]
        public async Task<Unit> Create([FromBody] Create.Command command)
            => await _mediator.Send(command with { ProfileRole = CurrentUser.Role , ProfileId =  CurrentUser.ProfileId});
    }

    public class Create
    {
        public record Command : IRequest
        {
            public string Key { get; init; }
            public string Name { get; init; }
            public string Avatar { get; init; }
            
            public Guid? ProfileId { get; init; }

            public ProfileRoles ProfileRole { get; init; }

        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            protected override async Task Handle(Command msg, CancellationToken token)
            {
                var segment = _db.Add(new Segment
                {
                    Name = msg.Name.Trim(),
                    Key = msg.Key.Trim(),
                    Avatar = msg.Avatar
                }).Entity;

                var team = _db.Add(new Team
                {
                    Segment = segment,
                    Name = "Team 1",
                    Key = "T1"
                }).Entity;

                if (msg.ProfileId != null && msg.ProfileRole != ProfileRoles.Admin)
                {
                    _db.Add(new ProfileAssignment
                    {
                        ProfileId = msg.ProfileId.Value,
                        SegmentId = segment.Id,
                        Team = team,
                    });
                }
                
                await _db.SaveChangesAsync(token);
            }
        }
    }
}