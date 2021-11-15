using System;
using System.Threading;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Organizations;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Profile
{
    public partial class ProfilesController
    {
        [HttpPut("togglePersonalAnalytics")]
        public async Task TogglePersonalAnalytics()
            => await _mediator.Send(new ToggleAnalytics.Command { ProfileId = CurrentUser.ProfileId});
    }
    
    public class ToggleAnalytics
    {
        public record Command: IRequest
        {
            public Guid ProfileId { get; init; }
        }
        
        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            protected override async Task Handle(Command msg, CancellationToken token)
            {
                var profile = await _db.Profiles.FirstOrDefaultAsync(x => x.Id == msg.ProfileId, token);

                profile.EnsureNotNull(msg.ProfileId);

                profile.IsAnalyticsEnabled = !profile.IsAnalyticsEnabled;

                await _db.SaveChangesAsync(token);
            }
        }
    }
}