using System;
using Finbuckle.MultiTenant;

namespace Tayra.Models.Catalog
{
    public class TenantModel : ITenantInfo
    {
        public string Id { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string Timezone { get; set; }
        public string ServicePlan { get; set; }

        public TenantModel() {}
        
        public TenantModel(string id, string identifier, string name, string connectionString, string timezone, string servicePlan)
        {
            Id = id;
            Identifier = identifier;
            Name = name;
            ConnectionString = connectionString;
            Timezone = timezone;
            ServicePlan = servicePlan;
        }

        public static TenantModel GetDummy()
        {
            return new (Guid.NewGuid().ToString(), "dummy", "dummy", "Data Source=tcp:localhost,1433;Initial Catalog=tayra-catalog;User ID=sa;Password=strong!Password;Connect Timeout=100;Encrypt=True;TrustServerCertificate=True;", "dummy", "dummy");
        }
        
        public static TenantModel WithConnectionStringOnly(string connectionString)
        {
            return new() {ConnectionString = connectionString};
        }
    }
}