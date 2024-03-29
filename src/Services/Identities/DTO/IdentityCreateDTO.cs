﻿using Tayra.Common;

namespace Tayra.Services
{
    public class IdentityCreateDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TenantHost { get; set; }

        public ProfileCreateDTO Profile { get; set; }
        
        public class ProfileCreateDTO
        {
            public string Username { get; set; }

            public string Avatar { get; set; }

            public ProfileRoles Role { get; set; }
        }
    }
}
