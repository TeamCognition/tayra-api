using System;
using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public class ProfileViewDTO
    {
        public int ProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        public ProfileRoles Role { get; set; }
        public TeamDTO[] Teams { get; set; }
        public string Avatar { get; set; }

        public double? Power { get; set; }
        public double? Speed { get; set; }
        public double? OImpact { get; set; }
        public HeatDTO Heat { get; set; }

        public double CompanyTokens { get; set; }
        public int Experience { get; set; }
        public double AverageScore { get; set; }
        public int Praises { get; set; }

        public IList<ItemActiveDTO> Badges { get; set; }
        public ItemActiveDTO Title { get; set; }
        public ItemActiveDTO Border { get; set; }

        public DateTime? LastUppedAt { get; set; }
        
        public class TokenDTO
        {
            public TokenType Type { get; set; }
            public double Value { get; set; }
        }

        public class HeatDTO
        {
            public int LastDateId { get; set; }
            public float[] Values { get; set; }
        }

        public class TeamDTO
        {
            public string Key { get; set; }
            public string Name { get; set; }
        }
    }
}