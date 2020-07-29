namespace Tayra.Services
{
    public class ProfileActivityChartDTO
    {
        public int DateId { get; set; }
        public AssistsDTO AssistsData { get; set; }
        public DeliveryDTO DeliveryData { get; set; }
        public ItemActivityDTO ItemActivityData { get; set; }
        //public QuestDTO QuestsData { get; set; }

        public GitCommitDTO[] GitCommitData { get; set; }

        public class AssistsDTO
        {
            public string[] EndorsedBy { get; set; }
            public string[] Endorsed { get; set; }
        }

        public class DeliveryDTO
        {
            public string[] TaskName { get; set; }
            public double TokensGained { get; set; }
        }

        public class ItemActivityDTO
        {
            public string[] Bought { get; set; }
            public string[] Disenchanted { get; set; }
            public string[] GiftsReceived { get; set; }
            public string[] GiftsSent { get; set; }
        }

        public class QuestsDTO
        {
            public string[] CommittedTo { get; set; }
            public string[] GoalsCompleted { get; set; }
            public string[] Completed { get; set; }
        }

        public class GitCommitDTO
        {
            public string Message { get; set; }
            public string ExternalUrl { get; set; }
        }
    }
}