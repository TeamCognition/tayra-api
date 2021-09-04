using System;
using System.Threading;
using System.Threading.Tasks;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Segments
{
    public partial class SegmentsController
    {
        [HttpDelete("removeMember")]
        public async Task<Unit> RemoveMember([FromQuery] Guid profileId, [FromQuery] Guid teamId, [FromQuery] Guid segmentId)
            => await _mediator.Send(new RemoveMember.Command  {ProfileId = profileId, TeamId = teamId , SegmentId =  segmentId});
    }

    public class RemoveMember
    {
        public record Command : IRequest
        {
            public Guid ProfileId { get; init; }
            public Guid? SegmentId { get; init; }
            public Guid? TeamId { get; init; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            protected override async Task Handle(Command msg, CancellationToken token)
            {
                var profile = await _db.Profiles.FirstOrDefaultAsync(x => x.Id == msg.ProfileId, token);

                profile.EnsureNotNull(msg.ProfileId);

                if (!SegmentRules.CanRemoveProfileToSegment(profile.Role, msg.TeamId))
                {
                    throw new ApplicationException("If you are removing a member you must provide a teamId");
                }

                if (msg.TeamId.HasValue)
                {
                    var team = await _db.Teams.FirstOrDefaultAsync(x => x.Id == msg.TeamId,token);
                    team.EnsureNotNull(msg.TeamId);
                }
                else if (msg.SegmentId.HasValue)
                {
                    if (profile.Role != ProfileRoles.Manager)
                        throw new ApplicationException("only managers can be in segment without a team");

                    var segment = await _db.Segments.FirstOrDefaultAsync(x => x.Id == msg.SegmentId, token);
                    segment.EnsureNotNull(msg.SegmentId);
                }
                else
                {
                    throw new ApplicationException("you have to provide either segmentId or teamId");
                }

                var profileAssignment =
                    await _db.ProfileAssignments.FirstOrDefaultAsync(
                        x => x.ProfileId == msg.ProfileId && x.TeamId == msg.TeamId, token);
                
                _db.Remove(profileAssignment);
                
                await _db.SaveChangesAsync(token);
            }
        }
    }
}