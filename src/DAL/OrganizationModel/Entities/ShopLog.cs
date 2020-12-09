using System;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ShopLog
    {
        //Composite Key
        public Guid ShopId { get; set; }
        public virtual Shop Shop { get; set; }

        public Guid LogId { get; set; }
        public virtual Log Log { get; set; }

        public LogEvents Event { get; set; }
    }
}
