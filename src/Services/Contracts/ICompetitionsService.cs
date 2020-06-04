using System.Collections.Generic;
using Cog.Core;

namespace Tayra.Services
{
    public interface ICompetitionsService
    {
        GridData<CompetitionViewGridDTO> GetSegmentCompetitionsGrid(int segmentId, CompetitionViewGridParams gridParams);
        List<CompetitionViewCompetitorDTO> GetActiveCompetitions(int profileId);
        GridData<CompetitionGridDTO> GetGridData(CompetitionGridParams gridParams);
        void Create(int segmentId, CompetitionCreateDTO dto);
        void AddCompetitors(int competitionId, IList<CompetitionAddCompetitorDTO> dto);
        void StartCompetition(int competitionId, CompetitionStartDTO dto);
        void EndCompetition(int competitionId);
        void StopCompetition(int competitionId);
    }
}
