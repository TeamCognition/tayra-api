using System;

namespace Cog.Core
{
    public class TimelineValue : Value
    {
        public TimelineValue(double? current, double? previous) : base(current, previous)
        {
        }

        public DateTime CurrentDay { get; set; }

        public DateTime PreviousDay { get; set; }
    }
}