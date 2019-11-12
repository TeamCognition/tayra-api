using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Firdaws.Core;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services;
using Task = System.Threading.Tasks.Task;

namespace Tayra.Auth
{
    public class ProfileService : IProfileService
    {
        protected readonly OrganizationDbContext DbContext;
        protected readonly IProfilesService ProfilesService;

        public ProfileService(OrganizationDbContext dbContext, IProfilesService profilesService)
        {
            DbContext = dbContext;
            ProfilesService = profilesService;
        }

        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
        /// </summary>
        public virtual Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject).Value;
            var profile = ProfilesService.GetByUsername(subject);

            if (profile == null)
            {
                profile = ProfilesService.GetByEmail(subject);
            }

            if (profile == null)
            {
                return Task.FromResult(0);
            }

            var team = DbContext.TeamMembers.Where(x => x.ProfileId == profile.Id)
                                            .Select(x => x.Team)
                                            .FirstOrDefault();

            var claimList = new List<Claim>();

            if(team != null)
            {
                claimList.Add(new Claim(TayraClaimTypes.TeamKey, team.Key));
            }

            claimList.Add(new Claim(FirdawsClaimTypes.ProfileId, profile.Id.ToString())); //For CreatedBy column
            claimList.Add(new Claim(ClaimTypes.Role, profile.Role.ToString()));

            context.IssuedClaims = claimList;
            return Task.FromResult(0);
        }

        /// <summary>
        /// This method gets called whenever identity server needs to determine if the user is valid or active (e.g. if the user's account has been deactivated since they logged in).
        /// (e.g. during token issuance or validation).
        /// </summary>
        public virtual Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;

            return Task.FromResult(0);
        }
    }
}