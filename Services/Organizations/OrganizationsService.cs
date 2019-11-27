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
        protected readonly ListShardMap<int> ShardMap;

        public OrganizationsService(CatalogDbContext catalogDb, ITenantProvider tenantProvider)
        {
            CatalogDb = catalogDb;
            ShardMap = tenantProvider.GetShardMap() as ListShardMap<int>;
        }

        // Enter a new shard - i.e. an empty database - to the shard map, allocate a first tenant to it 
        // and kick off EF intialization of the database to deploy schema
        // public void RegisterNewShard(string server, string database, string user, string pwd, string appname, int key)

        public void Create(OrganizationCreateDTO dto)
        {
            if (ShardMap == null)
                throw new Exception("Shard map not initialized");

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
                Name = dto.Key,
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

        public OrganizationDTO GetVenueDetails(int tenantId)
        {
            //get database name
            //string databaseName, databaseServerName;
            //PointMapping<int> mapping;

            //if (ShardMap.TryGetMappingForKey(tenantId, out mapping))
            //{
            //    using (SqlConnection sqlConn = ShardMap.OpenConnectionForKey(tenantId, _connectionString))
            //    {
            //        databaseName = sqlConn.Database;
            //        databaseServerName = sqlConn.DataSource.Split(':').Last().Split(',').First();
            //    }
            //    var venue = DbContext.Organizations.FirstOrDefault(x => x.Id == tenantId);

            //    if (venue != null)
            //    {
            //        var venueModel = new OrganizationDTO
            //        {
            //            Id = venue.Id,
            //            Name = venue.Name,
            //            Address = venue.Address,
            //            DatabaseName = databaseName,
            //            DatabaseServerName = databaseServerName
            //        };
            //        return venueModel;
            //    }
            //}
            return null;
        }
    }
}
