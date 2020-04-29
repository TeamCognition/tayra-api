using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ShopLog
    {
        //Composite Key
        public int ShopId { get; set; }
        public virtual Shop Shop { get; set; }

        public int LogId { get; set; }
        public virtual Log Log { get; set; }

        public LogEvents Event { get; set; }
    }
}
