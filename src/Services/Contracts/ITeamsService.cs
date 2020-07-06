using System.Collections.Generic;
using Cog.Core;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface ITeamsService
    {
        TeamViewDTO GetTeamViewDTO(string teamKey);
        TeamRawScoreDTO GetTeamRawScoreDTO(string teamKey);
        TeamSwarmPlotDTO GetTeamSwarmPloteDTO(string teamKey);
        GridData<TeamViewGridDTO> GetViewGridData(int[] teamIds, TeamViewGridParams gridParams);
        GridData<TeamMembersGridDTO> GetTeamMembersGridData(TeamMembersGridParams gridParams);
        TeamImpactPieChartDTO GetImpactPieChart(int teamId);
        TeamImpactLineChartDTO GetImpactLineChart(int teamId);
        void Create(int segmentId, TeamCreateDTO dto);
        void Update(int teamId, TeamUpdateDTO dto);
        void Archive(int profileId, string teamKey);
    }
}