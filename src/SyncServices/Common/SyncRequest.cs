using System;
using System.Collections.Generic;

namespace Tayra.SyncServices.Common
{
    public class SyncRequest
    {
        public DateTime? Date { get; set; }
        public string OrganizationKey { get; set; }
        public string TimezoneId { get; set; }

        public Dictionary<string, string> Params { get; set; }
    }
}
