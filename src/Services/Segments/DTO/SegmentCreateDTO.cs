using System.ComponentModel.DataAnnotations;

namespace Tayra.Services
{
    public class SegmentCreateDTO
    {
        [Required, MaxLength(50)]
        public string Key { get; set; }

        public string Avatar { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}
