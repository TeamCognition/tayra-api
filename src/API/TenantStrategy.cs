using System;
using System.Threading.Tasks;
using Cog.Core;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Http;

namespace Tayra.API
{
    public class TenantStrategy : IMultiTenantStrategy
    {
        public async Task<string> GetIdentifierAsync(object context)
        {
            if (!(context is HttpContext httpContext))
                throw new MultiTenantException(null,
                    new ArgumentException($@"""{nameof(context)}"" type must be of type HttpContext", nameof(context)));

            var tenantIdentifier = new CogPrincipal(httpContext?.User)?.CurrentTenantKey;

            return await Task.FromResult(tenantIdentifier);
        }
    }
}