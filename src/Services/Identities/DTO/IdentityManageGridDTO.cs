using System;
using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public class IdentityManageGridDTO
    {
        public int ProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public ProfileRoles Role { get; set; }
        public DateTime JoinedAt { get; set; }

        public ICollection<IntegrationDTO> Integrations { get; set; }

        public class IntegrationDTO
        {
            public IntegrationType Type { get; set; }
        }
    }
}
