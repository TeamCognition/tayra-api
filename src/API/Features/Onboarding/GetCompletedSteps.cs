using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Organizations;
using Tayra.Services._Models.Onboarding;
using Result = System.Collections.Generic.Dictionary<string, bool>;

namespace Tayra.API.Features.Onboarding
{
    public partial class OnboardingController
    {
        [HttpGet("completedSteps")]
        public async Task<Result> GetCompletedSteps()
            => await _mediator.Send(new GetCompletedSteps.Query{ProfileId = CurrentUser.ProfileId, TenantIdentifier = CurrentUser.CurrentTenantIdentifier});
    }
    
    public class GetCompletedSteps
    {
        public record Query : IRequest<Result>
        {
            public Guid ProfileId { get; init; }
            public string TenantIdentifier { get; init; }
        }
        
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var profileOnboarding = await _db.Profiles
                    .Where(x => x.Id == msg.ProfileId)
                    .Select(x => new
                    {
                        x.IsProfileOnboardingCompleted
                    }).FirstOrDefaultAsync(token);
            
                var tenantOnboarding = await _db.LocalTenants
                    .Where(x => x.Identifier == msg.TenantIdentifier)
                    .Select(x => new 
                    {
                        IsCreateSegmentCompleted = x.IsSegmentOnboardingCompleted,
                        IsAddSourcesCompleted = x.IsAppsOnboardingCompleted,
                        IsInviteUsersCompleted = x.IsAppsOnboardingCompleted
                    }).FirstOrDefaultAsync(token);
                
                return new Result
                {
                    {OnboardingStepIds.CreateProfile.ToString(), profileOnboarding.IsProfileOnboardingCompleted},
                    {OnboardingStepIds.CreateSegment.ToString(), tenantOnboarding.IsCreateSegmentCompleted},
                    {OnboardingStepIds.InstallApps.ToString(), tenantOnboarding.IsAddSourcesCompleted},
                    {OnboardingStepIds.InviteMembers.ToString(), tenantOnboarding.IsInviteUsersCompleted }
                };
            }
        }
    }
}