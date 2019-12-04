using System.Collections.Generic;
using Firdaws.Core;

namespace Tayra.Services
{
    public interface ITeamsService
    {
        TeamViewDTO GetTeamViewDTO(string teamId);
        GridData<TeamViewGridDTO> GetViewGridData(int projectId, TeamViewGridParams gridParams);
        GridData<TeamMembersGridDTO> GetTeamMembersGridData(TeamMembersGridParams gridParams);
        void Create(int projectId, TeamCreateDTO dto);
        void Update(int projectId, TeamUpdateDTO dto);
        void AddMembers(string teamKey, IList<TeamAddMemberDTO> dto);
        void Archive(int profileId, string teamKey);
    }
}
