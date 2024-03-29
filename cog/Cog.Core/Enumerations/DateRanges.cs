﻿using System.ComponentModel;

namespace Cog.Core
{
    public enum DateRanges
    {
        [Description("Custom")] Custom = 0,

        [Description("Yesterday")] Yesterday = 1,

        [Description("Last 7 Days")] Last7Days = 2,

        [Description("Last 30 Days")] Last30Days = 3,

        [Description("Last 28 Days")] Last28Days = 4,

        [Description("Last Week")] LastWeek = 5,

        [Description("Last 4 Week")] Last4Week = 5,

        [Description("Last Month")] LastMonth = 6,

        [Description("Last 8 Week")] Last8Week = 7
    }
}