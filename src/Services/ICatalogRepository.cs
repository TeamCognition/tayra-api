using System.Collections.Generic;
using Tayra.Models.Catalog;

namespace Tayra.Services
{
    public interface ICatalogRepository
    {
        List<TenantModel> GetAllTenants();
        TenantModel GetTenant(string tenantName);
        bool Add(Tenant tenant);
    }
}
