using System.ComponentModel;

namespace Cog.Core
{
    public enum DateRanges
    {
        [Description("Yesterday")]
        Yesterday = 1,

        [Description("Last 7 Days")]
        Last7Days = 2,

        [Description("Last 30 Days")]
        Last30Days = 3,

        [Description("Last Week")]
        LastWeek = 4,

        [Description("Last Month")]
        LastMonth = 5,

        [Description("Custom")]
        Custom = 6
    }
}