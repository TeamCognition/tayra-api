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
        GridData<TeamProfilesGridDTO> GetTeamProfilesGridData(TeamProfilesGridParams gridParams);
        void Create(int segmentId, TeamCreateDTO dto);
        void Update(int teamId, TeamUpdateDTO dto);
        void Archive(int profileId, string teamKey);
        TeamStatsDTO GetTeamStatsData(string teamKey);
        TeamPulseDTO GetTeamPulse(string teamKey);
    }
}