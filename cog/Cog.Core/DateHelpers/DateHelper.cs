using System;

namespace Cog.Core
{
    public static class DateHelper
    {
        public static DateRange FindPeriod(DateRanges range)
        {
            var today = DateTime.Today;
            var yesterday = today.Subtract(TimeSpan.FromDays(1));
            switch (range)
            {
                case DateRanges.Custom:
                    {
                        return null;
                    }
                case DateRanges.Last30Days:
                    {
                        return new DateRange(today.Subtract(TimeSpan.FromDays(30)), yesterday);
                    }
                case DateRanges.Last7Days:
                    {
                        return new DateRange(today.Subtract(TimeSpan.FromDays(7)), yesterday);
                    }
                case DateRanges.LastMonth:
                    {
                        var aMonthAgo = today.Subtract(TimeSpan.FromDays(30));
                        var start = new DateTime(aMonthAgo.Year, aMonthAgo.Month, 1);
                        var end = start.AddMonths(1).Subtract(TimeSpan.FromSeconds(1));
                        return new DateRange(start, end);
                    }
                case DateRanges.LastWeek:
                    {
                        var aWeekAgo = today.Subtract(TimeSpan.FromDays(7));
                        var start = today.Subtract(TimeSpan.FromDays((int)aWeekAgo.DayOfWeek));
                        var end = start.AddDays(7).Subtract(TimeSpan.FromSeconds(1));
                        return new DateRange(start, end);
                    }
                default:
                    {
                        return new DateRange(yesterday, yesterday);
                    }
            }
        }

        public static DateRange FindComparePeriod(DateRange period, ComparisonPeriods? comparison)
        {
            if (period == null)
            {
                return null;
            }

            return FindComparePeriod(period.From, period.To, comparison);
        }

        public static DateRange FindComparePeriod(DateTime startDate, DateTime endDate, ComparisonPeriods? comparison)
        {
            if (comparison == null)
            {
                return null;
            }

            switch (comparison)
            {
                case ComparisonPeriods.PreviousYear:
                    {
                        DateTime AdjustDate(DateTime date)
                        {
                            var year = date.Year - 1;
                            var month = date.Month;
                            var day = date.Day;

                            if (month == 2 && day > 28)
                            {
                                day = 28;
                            }

                            return new DateTime(year, month, day);
                        }

                        startDate = AdjustDate(startDate);
                        endDate = AdjustDate(endDate);
                        break;
                    }
                default:
                    {
                        var days = endDate.Subtract(startDate).Days + 1;
                        endDate = startDate.Subtract(TimeSpan.FromSeconds(1));
                        startDate = endDate.Subtract(TimeSpan.FromDays(days)).AddSeconds(1);
                        break;
                    }
            }

            return new DateRange(startDate, endDate);
        }
    }
}