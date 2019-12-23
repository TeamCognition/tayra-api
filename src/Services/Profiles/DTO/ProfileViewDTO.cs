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

        public IList<ActiveItem> Badges { get; set; }
        public ActiveItem Title { get; set; }

        public virtual IEnumerable<Project> Projects { get; set; }
        public virtual IEnumerable<Team> Teams { get; set; }
        public virtual IList<Token> CustomTokens { get; set; } //Ienumerable when fixed profileViewGET

        public class Project
        {
            public string Key { get; set; }
            public string Name { get; set; }
        }

        public class Team
        {
            public string Key { get; set; }
            public string Name { get; set; }
            public string Avatar { get; set; }
        }

        public class Token
        {
            public string Name { get; set; }
            public TokenType Type { get; set; }
            public double Value { get; set; }
        }

        public class ActiveItem
        {
            public int InventoryItemId { get; set; }
            public string Name { get; set; }
            public string Image { get; set; }
            public ItemTypes Type { get; set; }
        }
    }
}
