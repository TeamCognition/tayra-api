using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;

namespace Tayra.Models.Organizations
{
    public interface ITenantProvider
    {
        TenantDTO GetTenant();

        ShardMap GetShardMap();

        string GetTemplateConnectionString();
    }
}
