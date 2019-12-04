using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Tayra.DAL
{
    public static class ConnectionStringUtilities
    {
        public static string GetCatalogDbConnStr(IConfiguration config)
        {
            return GetSqlDatabaseConnectionString(config["CatalogServer"], config["CatalogDatabase"], config["DatabaseUser"], config["DatabasePassword"]);
        }

        /// <summary>
        /// Gets the full SQL connection string with a database.
        /// </summary>
        public static string GetSqlDatabaseConnectionString(string serverUrl, string databaseName, string userId, string password, int port = 1443, int timeout = 100, string protocol = "tcp")
        {
            return new SqlConnectionStringBuilder
            {
                DataSource = $"{serverUrl},{port}",
                InitialCatalog = databaseName,
                UserID = userId,
                Password = password,
                ConnectTimeout = timeout,
                Encrypt = true,
                

            }.ConnectionString;
        }

        /// <summary>
        /// Gets the SQL connection string with just user id and password.
        /// </summary>
        public static string GetSqlTemplateConnectionString(string userId, string password, int timeout = 100) =>
            new SqlConnectionStringBuilder
            {
                UserID = userId,
                Password = password,
                ConnectTimeout = timeout
            }.ConnectionString;
    }
}
