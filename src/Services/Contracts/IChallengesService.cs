using Firdaws.Core;

namespace Tayra.Services
{
    public interface IChallengesService
    {
        GridData<ChallengeViewGridDTO> GetChallengesGrid(ChallengeViewGridParams gridParams);
        GridData<ChallengeCommitGridDTO> GetChallengeCommitsGrid(int profileId, ChallengeCommitGridParams gridParams);
        ChallengeViewDTO GetChallengeViewDTO(int profileId, int challengeId);
        void Create(ChallengeCreateDTO dto);
        void Update(ChallengeUpdateDTO dto);
        void CompleteGoal(int profileId, ChallengeGoalCompleteDTO dto);
        void CommitToChallenge(int profileId, ChallengeCommitDTO dto);
        void CompleteChallenge(ChallengeCompleteDTO dto);
        void EndChallenge(int profileId, int challengeId);
    }
}