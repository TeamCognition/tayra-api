namespace Tayra.Services
{
    public class ChallengeUpdateDTO : ChallengeCreateDTO
    {
        public int ChallengeId { get; set; }
        public int? CompletionsRemaining { get; set; }

        //Doesn't update status
        //Doesn't update Completition Limit
    }
}
