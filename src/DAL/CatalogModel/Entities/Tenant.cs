using System.ComponentModel.DataAnnotations;

namespace Tayra.Models.Catalog
{
    public class Tenant
    {
        [Key]
        public byte[] Id { get; set; }

        [Required, MaxLength(50)]
        public string Key { get; set; }

        [MaxLength(50)]
        public string Timezone { get; set; }

        [MaxLength(100)]
        public string DisplayName { get; set; }

        //[MaxLength(4000)]
        //public string Database { get; set; }

        public string ServicePlan { get; set; }

        //public string DatebaseType { get; set; }
    }
}
