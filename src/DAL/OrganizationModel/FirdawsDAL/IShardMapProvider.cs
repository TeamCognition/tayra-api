using System.Data.SqlClient;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;

namespace Tayra.Models.Organizations
{
    public interface IShardMapProvider
    {
        ListShardMap<int> ShardMap { get; set; }
        string TemplateConnectionString { get; set; }
    }
}
