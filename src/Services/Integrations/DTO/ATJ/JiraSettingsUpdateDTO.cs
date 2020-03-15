using System.Collections.Generic;

namespace Tayra.Services
{
    public class JiraSettingsUpdateDTO
    {
        public ICollection<ActiveProject> ActiveProjects { get; set; }
        public bool PullTasksForNewProjects { get; set; }
    }
}
