namespace Tayra.Services
{
    public interface IUtilities
    {
        void RegisterTenantShard(TenantServerConfig tenantServerConfig, DatabaseConfig databaseConfig, CatalogConfig catalogConfig, bool resetEventDate);

        byte[] ConvertIntKeyToBytesArray(int key);
    }
}
