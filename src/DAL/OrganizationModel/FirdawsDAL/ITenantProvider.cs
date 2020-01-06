using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public interface ITenantProvider
    {
        TenantDTO GetTenant();
    }
}
