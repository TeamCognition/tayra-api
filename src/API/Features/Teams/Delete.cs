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
        [HttpDelete("{teamId}")]
        public async Task<Unit> DeleteTeam([FromRoute] Guid teamId)
            => await _mediator.Send(new Delete.Command  {TeamId = teamId, ProfileId = CurrentUser.ProfileId});
    }

    public class Delete
    {
        public record Command : IRequest
        {
            public Guid TeamId { get; init; }
            
            public Guid ProfileId { get; init; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            protected override async Task Handle(Command msg, CancellationToken token)
            {
                var team = await _db.Teams.Include(x => x.Members).FirstOrDefaultAsync(x => x.Id == msg.TeamId, token);

                team.EnsureNotNull(msg.TeamId);

                _db.Remove(team);

                foreach (var m in team.Members) //is this needed?
                {
                    _db.Remove(m);
                }
                
                await _db.SaveChangesAsync(token);
            }
        }
    }
}