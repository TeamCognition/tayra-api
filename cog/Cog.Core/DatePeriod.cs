using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cog.Core
{
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class DatePeriod
    {
        public DateTime From { get; }

        public DateTime To { get; }

        public int FromId => DateHelper2.ToDateId(From);
        public int ToId => DateHelper2.ToDateId(To);

        public DatePeriod(int fromId, int toId)
        {
            if (fromId > toId)
            {
                throw new ApplicationException("'from' must be smaller than 'to'");
            }

            From = DateHelper2.ParseDate(fromId);
            To = DateHelper2.ParseDate(toId);
        }

        public override string ToString() => $"{DateHelper2.ToDateId(From)}-{DateHelper2.ToDateId(To)}";

        public DatePeriod(DateTime from, DateTime to)
        {
            if (from > to)
            {
                throw new ApplicationException("'from' must be smaller than 'to'");
            }
            From = from;
            To = to;
        }
        
        public DatePeriod(string datePeriodString)
        {
            var dates = datePeriodString.Split('-');
            var from = DateHelper2.ParseDate(dates[0]);
            var to = DateHelper2.ParseDate(dates[1]);
            
            if (from > to)
            {
                throw new ApplicationException("'from' must be smaller than 'to'");
            }
            From = from;
            To = to;
        }
        
        public IEnumerable<DatePeriod> SplitToIterations(int iterationDaysCount = 7)
        {
            DateTime iterationFrom = From;
            DateTime iterationTo;
            do
            {
                var tempTo = iterationFrom.AddDays(iterationDaysCount - 1);
                iterationTo = tempTo >= To ? To : tempTo;
                yield return new DatePeriod(iterationFrom, iterationTo);
                iterationFrom = iterationTo.AddDays(1);
            } while (iterationTo < To);
        }
    }
}