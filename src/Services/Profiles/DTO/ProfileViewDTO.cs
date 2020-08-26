using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tayra.Common;

namespace Tayra.Services
{
    public class ProfileViewDTO
    {
        public int ProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        public PulseDTO Pulse { get; set; }
        
        public ProfileRoles Role { get; set; }
        public TeamDTO[] Teams { get; set; }
        public SegmentDTO[] Segments { get; set; }
        public string Avatar { get; set; }
        public double CompanyTokens { get; set; }
        public int Experience { get; set; }
        
        public string AssistantSummary { get; set; }

        public IList<ItemActiveDTO> Badges { get; set; }
        public ItemActiveDTO Title { get; set; }
        public ItemActiveDTO Border { get; set; }

        public DateTime? LastUppedAt { get; set; }

        public PraiseDTO[] Praises { get; set; }
        
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
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class SegmentDTO
        {
            public string Key { get; set; }
            public int Id { get; set; }
            public string Name { get; set; }
        }
        
        public class PulseDTO
        {
            public Task[] InProgress { get; set; }
            public Task[] RecentlyDone { get; set; }
            public class Task
            {
                public TaskStatuses Status { get; set; }
                public string Summary { get; set; }
                public string ExternalUrl { get; set; }
            }
        }
        
        public class PraiseDTO
        {
            public PraiseTypes Type { get; set; }
            public int Count { get; set; }
        }
    }
}