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
        protected readonly IShardMapProvider ShardMapProvider;
        protected ListShardMap<int> ShardMap => ShardMapProvider.ShardMap;

        public OrganizationsService(CatalogDbContext catalogDb, IShardMapProvider shardMapProvider)
        {
            CatalogDb = catalogDb;
            ShardMapProvider = shardMapProvider;
        }

        public void EnsureOrganizationsAreCreatedAndMigrated()
        {
            foreach (var sl in ShardMap.GetShards())
            {
                var cs = new SqlConnectionStringBuilder(ShardMapProvider.TemplateConnectionString)
                {
                    DataSource = sl.Location.DataSource,
                    InitialCatalog = sl.Location.Database
                };
                TenantUtilities.DatabaseEnsureCreatedAndMigrated(cs.ConnectionString);
            }
        }

        // Enter a new shard - i.e. an empty database - to the shard map, allocate a first tenant to it 
        // and kick off EF intialization of the database to deploy schema
        // public void RegisterNewShard(string server, string database, string user, string pwd, string appname, int key)

        public void Create(OrganizationCreateDTO dto)
        {
            ShardLocation shardLocation = new ShardLocation(dto.DatabaseServer, dto.DatabaseName, SqlProtocol.Tcp, 1433); //port number is necessary, otherwise shard can't be found

            if (!ShardMap.TryGetShard(shardLocation, out Shard shard))
            {
                shard = ShardMap.CreateShard(shardLocation);
            }

            SqlConnectionStringBuilder connStrBldr = new SqlConnectionStringBuilder(dto.TemplateConnectionString);
            connStrBldr.DataSource = dto.DatabaseServer;
            connStrBldr.InitialCatalog = dto.DatabaseName;

            // Go into a DbContext to trigger migrations and schema deployment for the new shard.
            // This requires an un-opened connection.
            TenantUtilities.DatabaseEnsureCreatedAndMigrated(connStrBldr.ConnectionString); //TODO: move this to program.cs or somewhere

            var shardingKey = TenantUtilities.GenerateShardingKey(dto.Key);

            // Register the mapping of the tenant to the shard in the shard map.
            // After this step, DDR on the shard map can be used
            if (!ShardMap.TryGetMappingForKey(shardingKey, out PointMapping<int> mapping))
            {
                mapping = ShardMap.CreatePointMapping(shardingKey, shard);
            }
            else
            {
                //mapping.value
                throw new Exception($"Tenant with {dto.Key} already exists in shard point mapping");
            }

            //convert from int to byte[] as tenantId has been set as byte[] in Tenants entity
            var tenantId = TenantUtilities.ConvertShardingKeyToTenantId(mapping.Value);

            CatalogDb.Add(new Tenant
            {
                Id = tenantId,
                Key = dto.Key,
                DisplayName = dto.Name,
                Timezone = dto.Timezone
            });

            CatalogDb.SaveChanges();

            Models.Organizations.OrganizationsService.InsertOrganization(connStrBldr.ConnectionString, new Organization
            {
                Id = shardingKey,
                Address = "Burch",
                Name = dto.Name
            });
        }
    }
}