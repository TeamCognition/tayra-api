using System.Collections.Generic;
using Firdaws.Core;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface ITeamsService
    {
        TeamViewDTO GetTeamViewDTO(string teamId);
        GridData<TeamViewGridDTO> GetViewGridData(int[] teamIds, TeamViewGridParams gridParams);
        GridData<TeamMembersGridDTO> GetTeamMembersGridData(TeamMembersGridParams gridParams);
        TeamImpactPieChartDTO GetImpactPieChart(int teamId);
        TeamImpactLineChartDTO GetImpactLineChart(int teamId);
        void Create(int segmentId, TeamCreateDTO dto);
        void Update(int teamId, TeamUpdateDTO dto);
        void Archive(int profileId, string teamKey);
    }
}
