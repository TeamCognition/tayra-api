using System;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ProfileLog
    {
        public Guid ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public Guid LogId { get; set; }
        public virtual Log Log { get; set; }

        public LogEvents Event { get; set; }
    }
}
