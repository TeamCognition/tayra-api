using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class CompetitionLog
    {
        //Composite Key
        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }

        public int LogId { get; set; }
        public virtual Log Log { get; set; }

        public LogEvents Event { get; set; }
    }
}
