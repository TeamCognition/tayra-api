using System;

namespace Tayra.Services
{
    public class TeamProfilesGridDTO
    {
        public Guid ProfileId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public double Speed { get; set; }
        public double Power { get; set; }
        public double Impact { get; set; }
        public DateTime MemberFrom { get; set; }
    }
}
