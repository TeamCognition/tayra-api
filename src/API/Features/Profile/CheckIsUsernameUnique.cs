using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tayra.Services.Models.Profiles;
using Tayra.Models.Organizations;

namespace Tayra.API.Features.Profile
{
    public partial class ProfilesController
    {
        [AllowAnonymous, HttpGet("isUsernameUnique")]
        public async Task<bool> IsUsernameUnique([FromQuery] string username)
            => await _mediator.Send(new CheckIsUsernameUnique.Query { Username = username});
    }
    
    public class CheckIsUsernameUnique
    {
        public record Query : IRequest<bool>
        {
            public string Username { get; init; }
        }
        
        public class Handler : IRequestHandler<Query, bool>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<bool> Handle(Query msg, CancellationToken token)
            {
                return await new ProfilesService().IsUsernameUnique(_db, msg.Username, token);
            }
        }
    }
}