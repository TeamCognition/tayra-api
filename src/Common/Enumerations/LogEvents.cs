namespace Tayra.Common
{
    public enum LogEvents
    {
        System = 1000,

        Profile = 2000,
        ProfilePraiseGiven = 2001,
        ProfilePraiseReceived = 2002,

        Competition = 3000,

        Issue = 4000, //task
        IssueStatusChange = 4001,
        StatusChangeToCompleted = 4002,

        Shop = 5000,
        ShopItemPurchased = 5001,

        InventoryItem = 6000,
        InventoryItemDisenchanted = 6001,
        InventoryItemGifted = 6002,
        InventoryItemGiftReceived = 6003,

        Quest = 7000,
        QuestCompleted = 7001,
        QuestGoalCompleted = 7002,
        
        CodeCommit = 8000,
        CodeCommitted = 8001
    }
}