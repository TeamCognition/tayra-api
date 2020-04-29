using System;
using System.ComponentModel.DataAnnotations;
using Tayra.Common;

namespace Tayra.Services
{
    public class ProfilePraiseDTO
    {
        public int ProfileId { get; set; }
        public PraiseTypes Type { get; set; } = PraiseTypes.OneUp;
        public DateTime? DemoDate  { get; set; }

        [MaxLength(140)]
        public string Message { get; set; }
    }
}
