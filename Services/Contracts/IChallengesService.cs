using Firdaws.Core;

namespace Tayra.Services
{
    public interface IChallengesService
    {
        GridData<ChallengeViewGridDTO> GetProjectChallengesGrid(int projectId, ChallengeViewGridParams gridParams);
        void Create(int projectId, ChallengeCreateDTO dto);
        void Update(int projectId, ChallengeUpdateDTO dto);
        void CompleteChallenge(ChallengeCompleteDTO dto);
        void EndChallenge(int profileId, int challengeId);
    }
}