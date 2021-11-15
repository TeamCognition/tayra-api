using Tayra.Common;

namespace Tayra.Services.Models.Profiles
{
    public class ProfilePulse
    {
        public Task[] InProgress { get; set; }
        public Task[] RecentlyDone { get; set; }
        public string JiraBoardUrl { get; set; }

        public class Task
        {
            public WorkUnitStatuses Status { get; set; }
            public string Summary { get; set; }
            public string ExternalUrl { get; set; }
        }
    }
}