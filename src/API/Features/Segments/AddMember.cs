using System;
using System.Collections.Generic;
using System.Threading;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Organizations;
using Tayra.Services;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Segments
{
    public partial class SegmentsController
    {
        [HttpPost("addMember")]
        public async Task AddMember([FromBody] AddMember.Command commands)
            => await _mediator.Send(commands);
    }

    public static class AddMember
    {
        public record CommandDto
        {
            public Guid ProfileId { get; init; }
            public Guid TeamId { get; init; }
        }

        public record Command : IRequest
        {
            public List<CommandDto> Commands { get; init; }
        }


        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            protected override async Task Handle(Command msg, CancellationToken token)
            {
                foreach (var cmnd in msg.Commands)
                {
                    var profile = await _db.Profiles.FirstOrDefaultAsync(x => x.Id == cmnd.ProfileId, token);
                    profile.EnsureNotNull(cmnd.ProfileId);

                    if (!SegmentRules.CanAddProfileToSegment(profile.Role, cmnd.TeamId))
                        throw new ApplicationException("If you are adding a member you must provide a teamId");

                    var team = await _db.Teams.FirstOrDefaultAsync(x => x.Id == cmnd.TeamId, token);
                    team.EnsureNotNull(cmnd.TeamId);

                    _db.Add(new ProfileAssignment
                    {
                        ProfileId = cmnd.ProfileId,
                        SegmentId = team.SegmentId,
                        TeamId = team.Id
                    });
                }

                await _db.SaveChangesAsync(token);
            }
        }
    }
}