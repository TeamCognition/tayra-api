using System;
using Cog.Core;

namespace Tayra.Services
{
    public interface ITeamsService
    {
        TeamViewDTO GetTeamViewDTO(string segmentKey, string teamKey);
        TeamRawScoreDTO GetTeamRawScoreDTO(Guid teamId);
        TeamSwarmPlotDTO GetTeamSwarmPloteDTO(Guid teamId);
        GridData<TeamViewGridDTO> GetViewGridData(Guid[] teamIds, TeamViewGridParams gridParams);
        GridData<TeamProfilesGridDTO> GetTeamProfilesGridData(TeamProfilesGridParams gridParams);
        void Create(Guid segmentId, TeamCreateDTO dto);
        void Update(Guid teamId, TeamUpdateDTO dto);
        void Archive(Guid profileId, Guid teamId);
        TeamStatsDTO GetTeamStatsData(Guid teamId);
        TeamPulseDTO GetTeamPulse(Guid teamId);
    }
}