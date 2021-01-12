using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services._Models.Onboarding;

namespace Tayra.API.Features.Onboarding
{
    public partial class OnboardingController
    {
        [HttpPost("completeStep")]
        public async Task<Unit> CompleteStep([FromBody] CompleteStep.Command command)
            => await _mediator.Send(command with {ProfileId = CurrentUser.ProfileId, TenantIdentifier = CurrentUser.CurrentTenantIdentifier});
    }
    
    public class CompleteStep
    {
        public record Command : IRequest
        {
            public OnboardingStepIds StepId { get; set; }
            public Guid ProfileId { get; set; }
            public string TenantIdentifier { get; set; }
        }
        
        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            protected override async System.Threading.Tasks.Task Handle(Command msg, CancellationToken token)
            {
                var o = _db.LocalTenants.FirstOrDefault(x => x.Identifier == msg.TenantIdentifier);
                var p = _db.Profiles.FirstOrDefault(x => x.Id == msg.ProfileId);
                
                switch (msg.StepId)
                {
                    case OnboardingStepIds.CreateProfile:
                        p.IsProfileOnboardingCompleted = true;
                        break;
                    case OnboardingStepIds.CreateSegment:
                        o.IsSegmentOnboardingCompleted = true;
                        break;
                    case OnboardingStepIds.InstallApps:
                        o.IsAppsOnboardingCompleted = true;
                        break;
                    case OnboardingStepIds.InviteMembers:
                        o.IsMembersOnboardingCompleted = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                await _db.SaveChangesAsync(token);
            }
        }
    }
}