using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Firdaws.Core;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;
using Task = System.Threading.Tasks.Task;

namespace Tayra.Auth
{
    public class ProfileService : IProfileService
    {
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly CatalogDbContext _catalogContext;
        private readonly IConfiguration _config;

        public ProfileService(IHttpContextAccessor httpAccessor, CatalogDbContext catalogContext, IConfiguration config)
        {
            _httpAccessor = httpAccessor;
            _catalogContext = catalogContext;
            _config = config;
        }

        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
        /// </summary>
        public virtual Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject).Value;

            var tenant = _catalogContext.TenantIdentities
                                        .Where(x => x.IdentityId == int.Parse(subject))
                                        .Select(x => x.Tenant)
                                        .FirstOrDefault();
            using (var orgContext = new OrganizationDbContext(null, new ShardTenantProvider(tenant.Name, _config)))
            {
                var profile = orgContext.Profiles
                    .FirstOrDefault(x => x.IdentityId == int.Parse(subject));

                if (profile == null)
                {
                    return Task.FromResult(0);
                }

                var team = orgContext.TeamMembers.Where(x => x.ProfileId == profile.Id)
                                                .Select(x => x.Team)
                                                .FirstOrDefault();

                var claimList = new List<Claim>();

                if (team != null)
                {
                    claimList.Add(new Claim(TayraClaimTypes.TeamKey, team.Key));
                }

                claimList.Add(new Claim(FirdawsClaimTypes.ProfileId, profile.Id.ToString())); //For CreatedBy column
                claimList.Add(new Claim(ClaimTypes.Role, profile.Role.ToString()));

                context.IssuedClaims = claimList;
                return Task.FromResult(0);
            }        
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