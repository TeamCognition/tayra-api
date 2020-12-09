using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Cog.Core;
using Cog.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.App
{
    class FakeTenantProvider : ITenantProvider
    {
        private string _key;

        [ActivatorUtilitiesConstructor]
        public FakeTenantProvider(IHttpContextAccessor accessor)
        {
            var principal = new TayraPrincipal(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(CogClaimTypes.CurrentTenantKey, "devtenant.tayra.local"),
                new Claim(CogClaimTypes.ProfileId, "1"),
                new Claim(CogClaimTypes.IdentityId, "1"),
                new Claim(ClaimTypes.Role, ProfileRoles.Admin.ToString())
            })));

            if (accessor.HttpContext.Request.Query.TryGetValue("tenant", out StringValues tenantKey))
            {
                _key = tenantKey;
            }
            else if (principal != null)
            {
                _key = principal.CurrentTenantKey;
            }
        }

        public FakeTenantProvider(string key)
        {
            _key = key;
        }

        public TenantDTO GetTenant()
        {
            return new TenantDTO
            {
                Key = _key,
                ShardingKey = TenantUtilities.GenerateShardingKey(_key)
            };
        }
    }
}