using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class IntegrationSegmentViewDTO
    {
        public IntegrationType Type { get; set; }
        public IntegrationStatuses Status { get; set; }
        public int MembersCount { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
    }
}
