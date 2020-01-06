using System;

namespace Tayra.Services
{
    public class TeamMembersGridDTO
    {
        private double _heat;
        
        public string Name { get; set; }
        public string Username { get; set; }
        public double Speed { get; set; }
        public double Heat
        {
            get { return _heat != 0 ? _heat : 22d; }
            set { _heat = value; }
        }
        public double Impact { get; set; }
        public DateTime MemberFrom { get; set; }
    }
}
