using System;

namespace Tayra.SyncServices.Common
{
    public class SyncRequest
    {
        public DateTime? Date { get; set; }
        public string OrganizationKey { get; set; }
        public string TimezoneId { get; set; }
    }
}
