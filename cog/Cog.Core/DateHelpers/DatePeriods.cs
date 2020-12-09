using System;
using System.Collections.Generic;

namespace Cog.Core
{
    public class DatePeriods
    {
        public DatePeriods(DateRanges range, DateTime? start, DateTime? end)
        {
            CurrentPeriod = DateHelper.FindPeriod(range);
            if (CurrentPeriod == null && start.HasValue & end.HasValue)
                CurrentPeriod = new DateRange(start.Value, end.Value);
        }

        public DatePeriods(DateRanges range, ComparisonPeriods? comparison, DateTime? start, DateTime? end)
        {
            CurrentPeriod = DateHelper.FindPeriod(range);
            if (CurrentPeriod == null && start.HasValue & end.HasValue)
                CurrentPeriod = new DateRange(start.Value, end.Value);

            ComparisonPeriod = DateHelper.FindComparePeriod(CurrentPeriod, comparison);
        }

        public DateRange CurrentPeriod { get; set; }

        public DateRange ComparisonPeriod { get; set; }

        public int? Duration => CurrentPeriod?.To.Subtract(CurrentPeriod.From).Days;

        public List<Tuple<DateTime, DateTime>> GetTimeLine()
        {
            if (CurrentPeriod == null || ComparisonPeriod == null) return null;

            var result = new List<Tuple<DateTime, DateTime>>();

            for (var i = 0; i <= Duration; i++)
                result.Add(Tuple.Create(CurrentPeriod.From.AddDays(i), ComparisonPeriod.From.AddDays(i)));

            return result;
        }
    }
}