using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tayra.Common;

namespace Tayra.Services
{
    public class PraiseProfileDTO
    {
        public int ProfileId { get; set; }
        public PraiseTypes Type { get; set; } = PraiseTypes.HardWorker;
        public DateTime? DemoDate { get; set; }

        [MaxLength(140)]
        public string Message { get; set; }
    }
}
