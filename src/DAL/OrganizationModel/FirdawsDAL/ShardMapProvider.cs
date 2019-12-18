using System;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using Microsoft.Extensions.Configuration;
using Tayra.DAL;

namespace Tayra.Models.Organizations
{
    public sealed class ShardMapProvider : IShardMapProvider
    {
        public ListShardMap<int> ShardMap { get; set; }

        public string TemplateConnectionString { get; set; }

        public ShardMapProvider(IConfiguration config)
        {
            ShardMap = CreateShardMap(config);
            TemplateConnectionString = ConnectionStringUtilities.GetSqlTemplateConnectionString(config);
        }

        /// <summary>
        /// Bootstrap Elastic Scale by creating a new shard map manager and a shard map on 
        /// the shard map manager database if necessary.
        /// </summary>
        private ListShardMap<int> CreateShardMap(IConfiguration config)
        {
            var connBuilder = new SqlConnectionStringBuilder(ConnectionStringUtilities.GetCatalogDbConnStr(config));
            
            string shardMapName = connBuilder.InitialCatalog;
            string connectionString = connBuilder.ConnectionString;

            try
            {
                // Deploy shard map manager
                // if shard map manager exists, refresh content, else create it, then add content
                ShardMapManager smm =
                    !ShardMapManagerFactory.TryGetSqlShardMapManager(connectionString,
                        ShardMapManagerLoadPolicy.Lazy, out smm)
                        ? ShardMapManagerFactory.CreateSqlShardMapManager(connectionString)
                        : smm;

                // check if shard map exists and if not, create it 
                return !smm.TryGetListShardMap(shardMapName, out ListShardMap<int> sm)
                    ? smm.CreateListShardMap<int>(shardMapName)
                    : sm;
            }
            catch (Exception exception)
            {
                Trace.TraceError(exception.Message, $"{shardMapName} : shardMapName \n {connectionString} : connectionString \n Error in sharding initialisation.");
                throw new ApplicationException("Error in sharding initialisation.");
            }
        }
    }
}
