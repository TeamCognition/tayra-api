using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Firdaws.Core;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Task = System.Threading.Tasks.Task;

namespace Tayra.Auth
{
    public class ProfileService : IProfileService
    {
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly CatalogDbContext _catalogContext;
        private readonly IShardMapProvider _shardMapProvider;

        public ProfileService(IHttpContextAccessor httpAccessor, CatalogDbContext catalogContext, IShardMapProvider shardMapProvider)
        {
            _httpAccessor = httpAccessor;
            _catalogContext = catalogContext;
            _shardMapProvider = shardMapProvider;
        }


        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0022:A catch clause that catches System.Exception and has an empty body", Justification = "<Pending>")]
        public virtual Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject).Value;

            var tenant = _catalogContext.TenantIdentities
                                        .Where(x => x.IdentityId == int.Parse(subject))
                                        .Select(x => x.Tenant)
                                        .FirstOrDefault();

            using (var orgContext = new OrganizationDbContext(null, new ShardTenantProvider(tenant.Key), _shardMapProvider)) //TODO: check if passing httpAccessor will change anything
            {
                var profile = orgContext.Profiles
                    .FirstOrDefault(x => x.IdentityId == int.Parse(subject));

                if (profile == null)
                {
                    try
                    {
                        orgContext.Add(new LoginLog
                        {
                            ProfileId = profile.Id,
                            IdentityId = profile.IdentityId,
                            FailReason = "In Auth.ProfileService, profile was null"
                        });
                        orgContext.SaveChanges();
                    }
                    catch (Exception)
                    {
                        throw new ApplicationException("Profile not found for identity " + subject);
                    }

                    return Task.FromResult(0);
                }

                (IQueryable<Segment> qs, IQueryable<Team> qt) = GetSegmentAndTeamQueries(orgContext, profile.Id, profile.Role);

                var claimList = new List<Claim>
                {
                    new Claim(FirdawsClaimTypes.CurrentTenantKey, tenant.Key),
                    new Claim(FirdawsClaimTypes.ProfileId, profile.Id.ToString()), //For CreatedBy column
                    new Claim(FirdawsClaimTypes.IdentityId, profile.IdentityId.ToString()),
                    new Claim(TayraClaimTypes.Role, profile.Role.ToString()),
                };

                claimList.AddRange(qs.Select(s => new Claim(TayraClaimTypes.Segment, s.Id.ToString())));
                claimList.AddRange(qt.Select(t => new Claim(TayraClaimTypes.Team, t.Id.ToString())));

                context.IssuedClaims = claimList;

                try
                {
                    orgContext.Add(new LoginLog
                    {
                        ProfileId = profile.Id,
                        IdentityId = profile.IdentityId,
                        ClaimsJson = JsonConvert.SerializeObject(claimList)
                    });
                    orgContext.SaveChanges();
                }
                catch (Exception) { }

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

        public static (IQueryable<Segment>, IQueryable<Team>) GetSegmentAndTeamQueries(OrganizationDbContext dbContext, int profileId, ProfileRoles role)
        {
            IQueryable<Segment> qs = dbContext.Segments;
            IQueryable<Team> qt = dbContext.Teams;

            if (role != ProfileRoles.Admin)
            {
                var segmentIds = dbContext.ProfileAssignments.Where(x => x.ProfileId == profileId).Select(x => x.SegmentId).Distinct().ToArray();
                qs = qs.Where(x => segmentIds.Contains(x.Id));

                if (role == ProfileRoles.Manager)
                {
                    qt = qt.Where(x => segmentIds.Contains(x.SegmentId));
                }
                else //is non-admin and non-manager. Is Member
                {
                    var teamIds = dbContext.ProfileAssignments.Where(x => x.ProfileId == profileId && x.TeamId.HasValue).Select(x => x.TeamId).ToArray();
                    qt = qt.Where(x => teamIds.Contains(x.Id));
                }
            }

            return (qs, qt);
        }
    }
}