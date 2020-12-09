using System;
using System.ComponentModel.DataAnnotations;
using Tayra.Common;

namespace Tayra.Models.Catalog
{
    public class TenantIntegration
    {
        public byte[] TenantId { get; set; }
        public Guid SegmentId { get; set; }
        public IntegrationType Type { get; set; }

        [MaxLength(125)]
        public string InstallationId { get; set; }

        public DateTime Created { get; set; }

        public Tenant Tenant { get; set; }
    }
}