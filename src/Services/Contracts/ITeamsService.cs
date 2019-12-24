using System.Collections.Generic;
using Firdaws.Core;

namespace Tayra.Services
{
    public interface ITeamsService
    {
        TeamViewDTO GetTeamViewDTO(string teamId);
        GridData<TeamViewGridDTO> GetViewGridData(int segmentId, TeamViewGridParams gridParams);
        GridData<TeamMembersGridDTO> GetTeamMembersGridData(TeamMembersGridParams gridParams);
        void Create(int segmentId, TeamCreateDTO dto);
        void Update(int segmentId, TeamUpdateDTO dto);
        void AddMembers(string teamKey, IList<TeamAddMemberDTO> dto);
        void Archive(int profileId, string teamKey);
    }
}
