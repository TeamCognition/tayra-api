namespace Tayra.Common
{
    public enum ActionPointTypes
    {
        ProfilesNoCompletedTasksIn7Days = 101,
        ProfilesNoCompletedTasksIn14Days = 102,

        ProfilesLowImpactFor2Weeks = 103,
        ProfilesHighImpactFor2Weeks = 104,

        ProfilesLowSpeedFor2Weeks = 105,
        ProfilesHighSpeedFor2Weeks = 106,

        ProfilesLowCommitRateFor2Weeks = 107,
        ProfilesHighCommitRateFor2Weeks = 108,

        ProfilesLowHeatFor2Weeks = 109,
        ProfilesHighHeatFor2Weeks = 110,

        ShopNoItemsAddedIn4Weeks = 201,

        ChallengeNotCreatedIn4Weeks = 302

    }
}
