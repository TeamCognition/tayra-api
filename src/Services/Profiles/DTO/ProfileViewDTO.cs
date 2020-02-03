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

        public string Avatar { get; set; }

        public float? Heat { get; set; }
        public float? Speed { get; set; }
        public float? OImpact { get; set; }

        public double CompanyTokens { get; set; }
        public int Experience { get; set; }
        public double AverageScore { get; set; }
        public int OneUps { get; set; }

        public IList<ItemActiveDTO> Badges { get; set; }
        public ItemActiveDTO Title { get; set; }
        public ItemActiveDTO Border { get; set; }

        public DateTime? LastUppedAt { get; set; }

        public virtual IList<Token> CustomTokens { get; set; } //Ienumerable when fixed profileViewGET

        public class Token
        {
            public string Name { get; set; }
            public TokenType Type { get; set; }
            public double Value { get; set; }
        }
    }
}