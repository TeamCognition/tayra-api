using Cog.Core;

namespace Tayra.Services
{
    public interface IQuestsService
    {
        GridData<QuestViewGridDTO> GetQuestsGrid(int[] segmentIds, QuestViewGridParams gridParams);
        GridData<QuestCommitsGridDTO> GetQuestCommitsGrid(int profileId, QuestCommitsGridParams gridParams);
        GridData<QuestCompletitionsGridDTO> GetQuestCompletitionsGrid(int profileId, QuestCompletitionsGridParams gridParams);
        QuestViewDTO GetQuestViewDTO(int profileId, int questId);
        void Create(QuestCreateDTO dto);
        void Update(int questId, QuestUpdateDTO dto);
        void CompleteGoal(int profileId, QuestGoalCompleteDTO dto);
        void CommitToQuest(int profileId, QuestCommitDTO dto);
        void CompleteQuest(QuestCompleteDTO dto);
        void EndQuest(int profileId, int questId);
    }
}