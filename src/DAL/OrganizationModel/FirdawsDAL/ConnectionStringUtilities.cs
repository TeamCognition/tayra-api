using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Tayra.DAL
{
    public static class ConnectionStringUtilities
    {
        public static string GetCatalogDbConnStr(IConfiguration config) =>
            GetSqlDatabaseConnectionString(config["CatalogServer"], config["CatalogDatabase"], config["DatabaseUser"], config["DatabasePassword"]).ConnectionString;

        /// <summary>
        /// Gets the full SQL connection string with a database.
        /// </summary>
        public static SqlConnectionStringBuilder GetSqlDatabaseConnectionString(string serverUrl, string databaseName, string userId, string password, int port = 1433, int timeout = 100, string protocol = "tcp")
        {
            return new SqlConnectionStringBuilder
            {
                DataSource = $"{protocol}:{serverUrl},{port}",
                InitialCatalog = databaseName,
                UserID = userId,
                Password = password,
                ConnectTimeout = timeout,
                Encrypt = true,
            };
        }

        public static string GetSqlTemplateConnectionString(IConfiguration config) =>
            GetSqlTemplateConnectionString(config["DatabaseUser"], config["DatabasePassword"]);

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
