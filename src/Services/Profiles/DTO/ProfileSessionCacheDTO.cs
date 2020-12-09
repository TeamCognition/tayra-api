using System;
using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public class ProfileSessionCacheDTO
    {
        public Guid ProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public ProfileRoles Role { get; set; }
        public bool IsAnalyticsEnabled { get; set; }

        public IList<ItemActiveDTO> Badges { get; set; }
        public ItemActiveDTO Title { get; set; }
        public ItemActiveDTO Border { get; set; }

        public ICollection<TeamDTO> Teams { get; set; }
        public ICollection<SegmentDTO> Segments { get; set; }

        public string TenantHost { get; set; }

        public class SegmentDTO
        {
            public Guid Id { get; set; }
            public string Key { get; set; }
            public string Name { get; set; }
            public string Avatar { get; set; }
        }

        public class TeamDTO
        {
            public Guid Id { get; set; }
            public string Key { get; set; }
            public string Name { get; set; }
            public string AvatarColor { get; set; }
            public Guid SegmentId { get; set; }
        }
    }
}