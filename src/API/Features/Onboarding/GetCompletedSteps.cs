using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tayra.Models.Organizations;
using Tayra.Services._Models.Onboarding;
using Result = System.Collections.Generic.Dictionary<string, bool>;

namespace Tayra.API.Features.Onboarding
{
    public partial class OnboardingController
    {
        [AllowAnonymous]
        [HttpGet("completedSteps")]
        public async Task<Result> GetCompletedSteps([FromQuery] GetCompletedSteps.Query query)
            => await _mediator.Send(query);
    }
    
    public class GetCompletedSteps
    {
        public record Query : IRequest<Result>
        {
            public Guid InvitationCode { get; init; }
            public string Tenant { get; init; }
        }
        
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var invitation = await _db.Invitations.FirstOrDefaultAsync(x => x.Code == msg.InvitationCode);

                var profileOnboarding = await _db.Profiles
                    .Where(x => x.Username == invitation.EmailAddress)
                    .Select(x => new
                    {
                        x.IsProfileOnboardingCompleted
                    }).FirstOrDefaultAsync(token);
                
                // In case the profile wasn't created yet
                bool isProfileOnboardingCompleted = profileOnboarding == null ? false : profileOnboarding.IsProfileOnboardingCompleted;

                var tenantOnboarding = await _db.LocalTenants
                    .Where(x => x.Identifier == msg.Tenant)
                    .Select(x => new 
                    {
                        IsCreateSegmentCompleted = x.IsSegmentOnboardingCompleted,
                        IsAddSourcesCompleted = x.IsAppsOnboardingCompleted,
                        IsInviteUsersCompleted = x.IsAppsOnboardingCompleted
                    }).FirstOrDefaultAsync(token);
                
                return new Result
                {
                    {OnboardingStepIds.CreateProfile.ToString(), isProfileOnboardingCompleted},
                    {OnboardingStepIds.CreateSegment.ToString(), tenantOnboarding.IsCreateSegmentCompleted},
                    {OnboardingStepIds.InstallApps.ToString(), tenantOnboarding.IsAddSourcesCompleted},
                    {OnboardingStepIds.InviteMembers.ToString(), tenantOnboarding.IsInviteUsersCompleted }
                };
            }
        }
    }
}