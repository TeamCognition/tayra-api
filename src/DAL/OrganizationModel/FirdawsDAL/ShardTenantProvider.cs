using System;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tayra.Models.Organizations
{
    public class ShardTenantProvider : ITenantProvider
    {
        private static ListShardMap<int> _shardMap { get; set; }
        private string _host;
        private IConfiguration _config;

        [ActivatorUtilitiesConstructor]
        public ShardTenantProvider(IHttpContextAccessor accessor, IConfiguration config)
        {
            _config = config;
            _host = accessor.HttpContext.Request.Host.ToString();
        }

        public ShardTenantProvider(string host, IConfiguration config)
        {
            _config = config;
            _host = host;
        }

        public ShardMap GetShardMap()
        {
            if (_shardMap == null)
                LoadShardMap();

            return _shardMap;
        }

        public TenantDTO GetTenant()
        {
            LoadShardMap();

            return new TenantDTO
            {
                Host = _host,
                ShardingKey = TenantUtilities.GenerateShardingKey(_host)
            };
        }

        /// <summary>
        /// Gets the basic SQL connection string.
        /// </summary>
        /// <returns></returns>
        public string GetTemplateConnectionString() =>
            new SqlConnectionStringBuilder
            {
                UserID = _config["DatabaseUser"],
                Password = _config["DatabasePassword"],
                ApplicationName = "Tayra",
                ConnectTimeout = Convert.ToInt32(_config["ConnectionTimeOut"])
            }.ConnectionString;


        private void LoadShardMap()
        {
            if (_shardMap != null)
            {
                return;
            }

            _shardMap = InitShardMap();
        }
        

        /// <summary>
        /// Bootstrap Elastic Scale by creating a new shard map manager and a shard map on 
        /// the shard map manager database if necessary.
        /// </summary>
        private ListShardMap<int> InitShardMap()
        {
            var basicConnectionString = GetTemplateConnectionString();

            SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder(basicConnectionString)
            {
                DataSource = SqlProtocol.Tcp + ":" + _config["CatalogServer"] + ".database.windows.net" + "," + Convert.ToInt32(_config["DatabaseServerPort"]),
                InitialCatalog = _config["CatalogDatabase"]
            };

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
                Trace.TraceError(exception.Message, "Error in sharding initialisation.");
                throw new ApplicationException("Error in sharding initialisation.");
            }
        }
    }
}
