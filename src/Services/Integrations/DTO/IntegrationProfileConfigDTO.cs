using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class IntegrationProfileConfigDTO
    {
        public int ProjectId { get; set; }
        public IntegrationType Type { get; set; }
        public string ExternalId { get; set; }
    }
}
