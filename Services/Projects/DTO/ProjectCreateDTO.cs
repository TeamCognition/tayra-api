﻿using System.ComponentModel.DataAnnotations;

namespace Tayra.Services
{
    public class ProjectCreateDTO
    {
        [Required, MaxLength(50)]
        public string Key { get; set; }

        public int OrganizationId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Timezone { get; set; }
    }
}
