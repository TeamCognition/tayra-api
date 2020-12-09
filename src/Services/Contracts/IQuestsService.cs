using System;
using Cog.Core;

namespace Tayra.Services
{
    public interface IQuestsService
    {
        GridData<QuestViewGridDTO> GetQuestsGrid(Guid[] segmentIds, QuestViewGridParams gridParams);
        GridData<QuestCommitsGridDTO> GetQuestCommitsGrid(Guid profileId, QuestCommitsGridParams gridParams);
        GridData<QuestCompletitionsGridDTO> GetQuestCompletitionsGrid(Guid profileId, QuestCompletitionsGridParams gridParams);
        QuestViewDTO GetQuestViewDTO(Guid profileId, int questId);
        void Create(QuestCreateDTO dto);
        void Update(int questId, QuestUpdateDTO dto);
        void CompleteGoal(Guid profileId, QuestGoalCompleteDTO dto);
        void CommitToQuest(Guid profileId, QuestCommitDTO dto);
        void CompleteQuest(QuestCompleteDTO dto);
        void EndQuest(Guid profileId, int questId);
    }
}