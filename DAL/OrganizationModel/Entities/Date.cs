using System;

namespace Tayra.Models.Organizations
{
    public class Date
    {
        public int Id { get; set; }

        public DateTime Day { get; set; }

        public string FullDateUK { get; set; }

        public string FullDateUSA { get; set; }

        public string DayOfMonth { get; set; }

        public string DaySuffix { get; set; }

        public string DayName { get; set; }

        public string DayOfWeekUSA { get; set; }

        public string DayOfWeekUK { get; set; }

        public string DayOfWeekInMonth { get; set; }

        public string DayOfWeekInYear { get; set; }

        public string DayOfQuarter { get; set; }

        public string DayOfYear { get; set; }

        public string WeekOfMonth { get; set; }

        public string WeekOfQuarter { get; set; }

        public string WeekOfYear { get; set; }

        public string Month { get; set; }

        public string MonthName { get; set; }

        public string MonthOfQuarter { get; set; }

        public string Quarter { get; set; }

        public string QuarterName { get; set; }

        public string Year { get; set; }

        public string YearName { get; set; }

        public string MonthYear { get; set; }

        public string MMYYYY { get; set; }

        public DateTime FirstDayOfMonth { get; set; }

        public DateTime LastDayOfMonth { get; set; }

        public DateTime FirstDayOfQuarter { get; set; }

        public DateTime LastDayOfQuarter { get; set; }

        public DateTime FirstDayOfYear { get; set; }

        public DateTime LastDayOfYear { get; set; }

        public bool IsHolidayUSA { get; set; }

        public bool IsWeekday { get; set; }

        public string HolidayUSA { get; set; }

        public bool IsHolidayUK { get; set; }

        public string HolidayUK { get; set; }
    }
}
