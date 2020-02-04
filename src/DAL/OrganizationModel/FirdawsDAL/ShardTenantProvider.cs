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
        private string _host;

        [ActivatorUtilitiesConstructor]
        public ShardTenantProvider(IHttpContextAccessor accessor)
        {
            var principal = new TayraPrincipal(accessor.HttpContext.User);

            if (principal.Identity.IsAuthenticated)
            {
                _host = principal.CurrentTenantKey;
            }
            else if(accessor.HttpContext.Request.Query.TryGetValue("tenant", out StringValues tenantKey))
            {
                _host = tenantKey;
            }
            else
            {
                throw new ApplicationException("No way to identify a tenant");
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

        public ShardTenantProvider(string host)
        {
            _host = host;
        }

        public TenantDTO GetTenant()
        {
            return new TenantDTO
            {
                Host = _host,
                ShardingKey = TenantUtilities.GenerateShardingKey(_host)
            };
        }
    }
}
