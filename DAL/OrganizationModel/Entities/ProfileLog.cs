using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ProfileLog
    {
        //Composite Key
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int LogId { get; set; }
        public virtual Log Log { get; set; }

        public LogEvents Event { get; set; }
    }
}
