using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class IntegrationProfileConfigDTO
    {
        public Guid Id { get; set; }
        public Guid SegmentId { get; set; }
        public IntegrationType Type { get; set; }
        public IntegrationStatuses Status { get; set; }
        public string ExternalId { get; set; }
    }
}
