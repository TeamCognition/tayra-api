using System.Data.SqlClient;

namespace Tayra.Common
{
    public static class ConnectionStringUtilities
    {
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
