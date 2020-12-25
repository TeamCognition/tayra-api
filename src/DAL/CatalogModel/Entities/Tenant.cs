using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tayra.Models.Catalog
{
    [Table("Tenants")]
    public class Tenant : TenantModel
    {
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// Host
        /// </summary>
        [Required, MaxLength(50)]
        public string Identifier { get; set; }
        
        [MaxLength(100)]
        public string Name { get; set; }

        public string ConnectionString { get; set; }
        
        [MaxLength(50)]
        public string Timezone { get; set; }
        
        public string ServicePlan { get; set; }
    }
}
