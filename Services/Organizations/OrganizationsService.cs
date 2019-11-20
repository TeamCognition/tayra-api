using System;
using System.Data.SqlClient;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class OrganizationsService : IOrganizationsService
    {
        protected readonly CatalogDbContext CatalogDb;

        public OrganizationsService(CatalogDbContext catalogDb)
        {
            CatalogDb = catalogDb;
        }

        // Enter a new shard - i.e. an empty database - to the shard map, allocate a first tenant to it 
        // and kick off EF intialization of the database to deploy schema
        // public void RegisterNewShard(string server, string database, string user, string pwd, string appname, int key)

        public void Create(OrganizationCreateDTO dto)
        {
            if (NewSharding.ShardMap == null)
                throw new Exception("Shard map not initialized");

            Shard shard;
            ShardLocation shardLocation = new ShardLocation(dto.DataBaseServer, dto.DatabaseName, SqlProtocol.Tcp);

            if (!NewSharding.ShardMap.TryGetShard(shardLocation, out shard))
            {
                shard = NewSharding.ShardMap.CreateShard(shardLocation);
            }

            SqlConnectionStringBuilder connStrBldr = new SqlConnectionStringBuilder(dto.TemplateConnectionString);
            connStrBldr.DataSource = dto.DataBaseServer;
            connStrBldr.InitialCatalog = dto.DatabaseName;

            // Go into a DbContext to trigger migrations and schema deployment for the new shard.
            // This requires an un-opened connection.
            TenantUtilities.DatabaseEnsureCreatedAndMigrated(connStrBldr.ConnectionString);

            var key = TenantUtilities.GenerateShardingKey(dto.Key);

            // Register the mapping of the tenant to the shard in the shard map.
            // After this step, DDR on the shard map can be used
            PointMapping<int> mapping;
            if (!NewSharding.ShardMap.TryGetMappingForKey(key, out mapping))
            {
                NewSharding.ShardMap.CreatePointMapping(key, shard);
            }

            //convert from int to byte[] as tenantId has been set as byte[] in Tenants entity
            var tenantId = TenantUtilities.ConvertShardingKeyToTenantId(mapping.Value);

            CatalogDb.Add(new Tenant
            {
                Id = tenantId,
                Name = dto.Key,
                DisplayName = dto.Name,
                Timezone = dto.Timezone
            });
        }
    }
}
