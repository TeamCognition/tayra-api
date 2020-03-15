using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using MoreLinq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;
using Task = System.Threading.Tasks.Task;

namespace Tayra.Auth
{
    public class CustomTokenRequestValidator : ICustomTokenRequestValidator
    {
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly CatalogDbContext _catalogContext;
        private readonly IShardMapProvider _shardMapProvider;


        public CustomTokenRequestValidator(IHttpContextAccessor httpAccessor, CatalogDbContext catalogContext, IShardMapProvider shardMapProvider)
        {
            _httpAccessor = httpAccessor;
            _catalogContext = catalogContext;
            _shardMapProvider = shardMapProvider;
        }

        public Task ValidateAsync(CustomTokenRequestValidationContext context)
        {
            if(context.Result.CustomResponse == null)
            {
                var subject = context.Result.ValidatedRequest.Subject.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject).Value;
                var currentTenantKey = _catalogContext.TenantIdentities
                            .Where(x => x.IdentityId == int.Parse(subject))
                            .Select(x => x.Tenant.Key)
                            .FirstOrDefault();


                var settings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                using (var orgContext = new OrganizationDbContext(null, new ShardTenantProvider(currentTenantKey), _shardMapProvider)) //TODO: check if passing httpAccessor will change anything
                {
                    context.Result.CustomResponse = new Dictionary<string, object>
                    {
                        { "sessionCache", JsonConvert.SerializeObject(GetSessionCache(orgContext, int.Parse(subject)), settings)}
                    };
                }

            }

            return Task.CompletedTask;
        }

        private static ProfileSessionCacheDTO GetSessionCache(OrganizationDbContext dbContext, int identityId)
        {
            var profile = (from p in dbContext.Profiles.Where(x => x.IdentityId == identityId)
                           select new ProfileSessionCacheDTO
                           {
                               ProfileId = p.Id,
                               FirstName = p.FirstName,
                               LastName = p.LastName,
                               Username = p.Username,
                               Role = p.Role,
                               Avatar = p.Avatar
                           }).FirstOrDefault();

            (IQueryable<Segment> qs, IQueryable<Team> qt) = ProfileService.GetSegmentAndTeamQueries(dbContext, profile.ProfileId, profile.Role);

            profile.Segments = qs.Select(s => new ProfileSessionCacheDTO.SegmentDTO
            {
                Id = s.Id,
                Key = s.Key,
                Name = s.Name,
                Avatar = s.Avatar
            }).ToArray();

            profile.Teams = qt.Select(t => new ProfileSessionCacheDTO.TeamDTO
            {
                Id = t.Id,
                Key = t.Key,
                Name = t.Name,
                AvatarColor = t.AvatarColor,
                SegmentId = t.SegmentId
            }).ToArray().Where(x => x.Key != null).ToArray();

            var activeItems = ProfilesService.GetProfileActiveItems(dbContext, profile.ProfileId);

            profile.Title = activeItems.Title;
            profile.Badges = activeItems.Badges;
            profile.Border = activeItems.Border;

            return profile;
        }
    }
}
