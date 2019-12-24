using Firdaws.Core;

namespace Tayra.Services
{
    public interface IChallengesService
    {
        GridData<ChallengeViewGridDTO> GetSegmentChallengesGrid(int segmentId, ChallengeViewGridParams gridParams);
        void Create(int segmentId, ChallengeCreateDTO dto);
        void Update(int segmentId, ChallengeUpdateDTO dto);
        void CompleteChallenge(ChallengeCompleteDTO dto);
        void EndChallenge(int profileId, int challengeId);
    }
}