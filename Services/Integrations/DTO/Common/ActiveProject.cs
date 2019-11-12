using System.ComponentModel.DataAnnotations;

namespace Tayra.Services
{
    public class ActiveProject
    {
        public ActiveProject(string projectId, string rewardStatusId)
        {
            ProjectId = projectId;
            RewardStatusId = rewardStatusId;
        }

        [Required]
        public string ProjectId { get; set; }

        [Required]
        public string RewardStatusId { get; set; }
    }
}
