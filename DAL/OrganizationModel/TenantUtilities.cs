using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Tayra.Models.Organizations
{
    public static class TenantUtilities
    {
        public static void DatabaseEnsureCreatedAndMigrated(string connectionString)
        {
            using (var x = new OrganizationDbContext(connectionString))
            {
                x.Database.Migrate();
            }
        }

        /// <summary>
        /// Generates tenant sharding key using MD5 Hashing.
        /// </summary>
        /// <param name="tenantName">Name of the tenant.</param>
        /// <returns></returns>
        public static int GenerateShardingKey(string tenantName)
        {
            var normalizedTenantName = tenantName.Replace(" ", string.Empty).ToLower();

            //Produce utf8 encoding of tenant name 
            var tenantNameBytes = Encoding.UTF8.GetBytes(normalizedTenantName);

            //Produce the md5 hash which reduces the size
            MD5 md5 = MD5.Create();
            var tenantHashBytes = md5.ComputeHash(tenantNameBytes);

            //Convert to integer for use as the id in the catalog 
            return BitConverter.ToInt32(tenantHashBytes, 0);

        }

        /// <summary>
        /// Converts the tenant id to bytes array.
        /// </summary>
        /// <param name="shardingKey">Tenant Sharding key.</param>
        /// <returns></returns>
        public static byte[] ConvertShardingKeyToTenantId(int shardingKey)
        {
            byte[] normalized = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(shardingKey));

            // Maps Int32.Min - Int32.Max to UInt32.Min - UInt32.Max.
            normalized[0] ^= 0x80;

            return normalized;
        }
    }
}
