using System;
using Cog.Core;

namespace Tayra.Services
{
    public interface ITeamsService
    {
        TeamViewDTO GetTeamViewDTO(string teamKey);
        TeamRawScoreDTO GetTeamRawScoreDTO(string teamKey);
        TeamSwarmPlotDTO GetTeamSwarmPloteDTO(string teamKey);
        GridData<TeamViewGridDTO> GetViewGridData(Guid[] teamIds, TeamViewGridParams gridParams);
        GridData<TeamProfilesGridDTO> GetTeamProfilesGridData(TeamProfilesGridParams gridParams);
        void Create(Guid segmentId, TeamCreateDTO dto);
        void Update(Guid teamId, TeamUpdateDTO dto);
        void Archive(Guid profileId, string teamKey);
        TeamStatsDTO GetTeamStatsData(Guid teamId);
        TeamPulseDTO GetTeamPulse(string teamKey);
    }
}