using System;
using System.Threading;
using System.Threading.Tasks;
using Cog.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tayra.Services.Models.Tokens;
using Tayra.Common;
using Tayra.Models.Organizations;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Profile
{
    public partial class ProfilesController
    {
        [HttpPost("modifyTokens")]
        public async Task<Unit> ModifyTokens([FromBody] ModifyTokens.Command command)
            => await _mediator.Send(command with {ProfileRole = CurrentUser.Role});
    }
    
    public class ModifyTokens
    {
        public record Command : IRequest
        {
            public Guid ProfileId { get; init; }
            public double TokenValue { get; init; }
            public ProfileRoles ProfileRole { get; init; }
        }
        
        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            protected override async Task Handle(Command msg, CancellationToken token)
            {
                if (msg.ProfileRole != ProfileRoles.Admin)
                {
                    throw new CogSecurityException("You are not allowed to perform this action!");
                }

                await new SendTokenTransactionService().Send(_db, TokenType.CompanyToken, msg.ProfileId, msg.TokenValue, TransactionReason.Manual, null, token);

                await _db.SaveChangesAsync(token);
            }
        }
    }
}