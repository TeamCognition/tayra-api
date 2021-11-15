using System.ComponentModel;

namespace Tayra.Common
{
    public enum ReportsType
    {
        [Description("Metrics: Impact, Speed, Assists, Commit Rate, Power, Effort")]
        Statistics = 1,
        [Description("Metrics: Heat, Completion, Complexity, Effort, Commits")]
        Workload = 2,
        [Description("Metrics: Praises Received, Praises Given, Tokens Earned, Tokens Spent, " +
                     "Inventory Value, Items Bought, Items Disenchanted, Gift Recieved, Gifts Sent, Time Logged By Tayra,  " +
                     "Time Logged By User")]
        TayraActivity = 3,
    }
}