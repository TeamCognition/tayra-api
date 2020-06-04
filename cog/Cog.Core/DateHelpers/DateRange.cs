using System;

namespace Cog.Core
{
    public class DateRange
    {
        public DateRange(DateTime fromDate, DateTime toDate)
        {
            From = fromDate;
            To = toDate;
            FromId = int.Parse(fromDate.ToString("yyyyMMdd"));
            ToId = int.Parse(toDate.ToString("yyyyMMdd"));
        }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public int FromId { get; set; }

        public int ToId { get; set; }
    }
}