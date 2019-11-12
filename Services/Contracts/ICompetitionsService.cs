using System.Collections.Generic;
using Firdaws.Core;

namespace Tayra.Services
{
    public interface ICompetitionsService
    {
        GridData<CompetitionViewGridDTO> GetProjectCompetitionsGrid(int projectId, CompetitionViewGridParams gridParams);
        List<CompetitionViewCompetitorDTO> GetActiveCompetitions(int profileId);
        GridData<CompetitionGridDTO> GetGridData(CompetitionGridParams gridParams);
        void Create(int projectId, CompetitionCreateDTO dto);
        void AddCompetitors(int competitionId, IList<CompetitionAddCompetitorDTO> dto);
        void StartCompetition(int competitionId, CompetitionStartDTO dto);
        void EndCompetition(int competitionId);
        void StopCompetition(int competitionId);
    }
}
