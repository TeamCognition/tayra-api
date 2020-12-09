using System;
using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public class LogCreateDTO
    {
        public LogEvents Event { get; set; }
        public Dictionary<string, string> Data { get; set; }
        public Guid? ProfileId { get; set; }
        public Guid? ShopId { get; set; }
    }
}
