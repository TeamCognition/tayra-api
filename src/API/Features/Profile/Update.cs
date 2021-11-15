using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Organizations;
using Tayra.Services;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Profile
{
    public partial class ProfilesController
    {
        [HttpGet, Route("update")]
        public async Task<Update.Command> GetUpdateProfile()
            => await _mediator.Send(new Update.Query { ProfileId = CurrentUser.ProfileId});
        
        
        [HttpPut("update")]
        public async Task<Unit> UpdateProfile([FromBody] Update.Command command)
            => await _mediator.Send(command with { ProfileId = CurrentUser.ProfileId});
        
    }
    
    public class Update
    {
        public record Query : IRequest<Command>
        {
            public Guid ProfileId { get; init; }
        }
        
        public record Command : IRequest
        {
            public Guid ProfileId { get; init; }
            public string Avatar { get; init; }
            public string FirstName { get; init; }
            public string LastName { get; init; }
            public string JobPosition { get; init; }
            public DateTime? BornOn { get; init; }
            public DateTime? EmployedOn { get; init; }
            public string Username { get; init; }
        }
        
        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly OrganizationDbContext _db;

            public QueryHandler(OrganizationDbContext db) => _db = db;

            public async Task<Command> Handle(Query msg, CancellationToken token)
            {
                return await _db.Profiles.Where(x => x.Id == msg.ProfileId).Select(x => new Command
                {
                    Avatar = x.Avatar,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    JobPosition = x.JobPosition,
                    BornOn = x.BornOn,
                    EmployedOn = x.EmployedOn,
                    Username = x.Username
                }).FirstOrDefaultAsync(token);
            }
        }
        
        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly OrganizationDbContext _db;

            public CommandHandler(OrganizationDbContext db) => _db = db;

            protected override async Task Handle(Command msg, CancellationToken token)
            {
                var profile = await _db.Profiles.FirstOrDefaultAsync(x => x.Id == msg.ProfileId, token);

                profile.EnsureNotNull(msg.ProfileId);

                profile.Avatar = msg.Avatar;
                profile.FirstName = msg.FirstName;
                profile.LastName = msg.LastName;
                profile.JobPosition = msg.JobPosition;
                profile.BornOn = msg.BornOn;
                profile.EmployedOn = msg.EmployedOn;
                profile.Username = msg.Username;

                await _db.SaveChangesAsync(token);
            }
        }
    }
}