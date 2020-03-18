using System;
using Firdaws.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ShardTenantProvider : ITenantProvider
    {
        private string _key;

        [ActivatorUtilitiesConstructor]
        public ShardTenantProvider(IHttpContextAccessor accessor)
        {
            var principal = new TayraPrincipal(accessor.HttpContext.User);
            if (principal.Identity.IsAuthenticated)
            {
                _key = principal.CurrentTenantKey;
            }
            else if(accessor.HttpContext.Request.Query.TryGetValue("tenant", out StringValues tenantKey))
            {                
                _key = tenantKey;
            }
            else
            {
                throw new ApplicationException("No way to identify the tenant");
            }
        
            //if(accessor.HttpContext.Request.Headers.TryGetValue("Origin", out StringValues origin))
            //{
            //    _host = new Uri(origin).Host;
            //if (_host.Contains("localhost") || _host.Contains("azurewebsites.net"))
            //{
            //    _host = "localhost:3000";
            //}
            //}
            //else
            //{
            //    throw new ApplicationException("No origin header provided");
            //}
        }

        public ShardTenantProvider(string key)
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
