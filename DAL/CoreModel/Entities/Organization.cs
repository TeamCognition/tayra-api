using System.ComponentModel.DataAnnotations;

namespace Tayra.Models.Core
{
    public class Organization
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string Key { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Timezone { get; set; }

        [MaxLength(4000)]
        public string Database { get; set; }

    }
}
