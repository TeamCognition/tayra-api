using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class LogDevice
    {
        public int Id { get; set; }

        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public LogDeviceTypes Type { get; set; }
        public string Address { get; set; }
        public int? DeliveryDelay { get; set; }
        //public bool IsEnabled { get; set; }

        public virtual ICollection<LogSetting> Settings { get; set; }
    }
}
