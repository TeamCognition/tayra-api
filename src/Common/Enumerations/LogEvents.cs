namespace Tayra.Common
{
    public enum LogEvents
    {
        System = 1000,

        Profile = 2000,
        ProfileOneUpGave = 2001,
        ProfileOneUpReceived = 2002,

        Competition = 3000,

        Issue = 4000,
        IssueStatusChange = 4001,
        StatusChangeToCompleted = 4002,

        Shop = 5000,
        ShopItemPurchased = 5001,

        InventoryItem = 6000,
        InventoryItemDisenchanted = 6001,
        InventoryItemGifted = 6002,
        InventoryItemReceived = 6003,

        Challenge = 7000,
        ChallengeCompleted = 7001,
        ChallengeGoalCompleted = 7002
    }
}
