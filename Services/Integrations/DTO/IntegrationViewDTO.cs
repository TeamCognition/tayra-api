using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class IntegrationViewDTO
    {
        public IntegrationType Type { get; set; }
        public int CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
    }
}
