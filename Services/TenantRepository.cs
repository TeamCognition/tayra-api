using System.Data.SqlClient;
using System.Linq;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class TenantRepository : ITenantRepository
    {
        #region Private variables

        private readonly string _connectionString;

        #endregion

        #region Constructor

        public TenantRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region Public methods

        public OrganizationModel GetVenueDetails(int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                //get database name
                string databaseName, databaseServerName;
                PointMapping<int> mapping;

                if (Sharding.ShardMap.TryGetMappingForKey(tenantId, out mapping))
                {
                    using (SqlConnection sqlConn = Sharding.ShardMap.OpenConnectionForKey(tenantId, _connectionString))
                    {
                        databaseName = sqlConn.Database;
                        databaseServerName = sqlConn.DataSource.Split(':').Last().Split(',').First();
                    }
                    var venue = context.Organizations.FirstOrDefault(x => x.Id == tenantId);

                    if (venue != null)
                    {
                        var venueModel = new OrganizationModel
                        {
                            Id = venue.Id,
                            Name = venue.Name,
                            Address = venue.Address,
                            DatabaseName = databaseName,
                            DatabaseServerName = databaseServerName
                        };
                        return venueModel;
                    }
                }
                return null;
            }
        }

        #endregion

        #region Private methods

        private OrganizationDbContext CreateContext(int tenantId)
        {
            return new OrganizationDbContext(Sharding.ShardMap, tenantId, _connectionString);
        }

        #endregion
    }
}
