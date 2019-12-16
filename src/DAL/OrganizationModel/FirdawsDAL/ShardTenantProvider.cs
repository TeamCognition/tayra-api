using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Tayra.Models.Organizations
{
    public class ShardTenantProvider : ITenantProvider
    {
        private string _host;

        [ActivatorUtilitiesConstructor]
        public ShardTenantProvider(IHttpContextAccessor accessor)
        {
            _host = accessor.HttpContext.Request.Host.ToString();
            _host = _host.Replace("localhost:5000", "localhost:5001");
            _host = _host.Replace("localhost:3000", "localhost:5001");
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
