using System;
using System.Diagnostics;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;

namespace Tayra.Services
{
    //Supports one Shard map
    public static class NewSharding
    {
        //public ShardMapManager ShardMapManager { get; private set; }
        public static ListShardMap<int> ShardMap { get; private set; }

        /// <summary>
        /// Bootstrap Elastic Scale by creating a new shard map manager and a shard map on 
        /// the shard map manager database if necessary.
        /// </summary>
        /// <param name="shardMapName">you could use catalog database name</param>
        /// <param name="connectionString"></param>
        public static void InitShardMap(string shardMapName, string connectionString)
        {
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
                ListShardMap<int> sm;
                ShardMap = !smm.TryGetListShardMap(shardMapName, out sm)
                    ? smm.CreateListShardMap<int>(shardMapName)
                    : sm;
            }
            catch (Exception exception)
            {
                Trace.TraceError(exception.Message, "Error in sharding initialisation.");
            }
        }
    }
}