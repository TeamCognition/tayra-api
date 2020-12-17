using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cog.Core
{
    [JsonConverter(typeof(DatePeriodConverter))]
    public class DatePeriod
    {
        public DatePeriod(int fromId, int toId)
        {
            if (fromId > toId) throw new ApplicationException("'from' must be smaller than 'to'");

            From = DateHelper2.ParseDate(fromId);
            To = DateHelper2.ParseDate(toId);
        }

        public DatePeriod(DateTime from, DateTime to)
        {
            if (from > to) throw new ApplicationException("'from' must be smaller than 'to'");

            From = from.Date;
            To = to.Date;
        }

        public DatePeriod(string datePeriodString)
        {
            var dates = datePeriodString.Split('-');
            var from = DateHelper2.ParseDate(dates[0]);
            var to = DateHelper2.ParseDate(dates[1]);

            if (from > to) throw new ApplicationException("'from' must be smaller than 'to'");

            From = from;
            To = to;
        }

        public DateTime From { get; }

        public DateTime To { get; }

        public int FromId => DateHelper2.ToDateId(From);
        public int ToId => DateHelper2.ToDateId(To);

        public int DaysCount => (To - From).Days + 1;
        public int IterationsCount => (To - From).Days / 7 + 1;

        public int WorkingDaysCount
        {
            get
            {
                var days = DaysCount;
                return WorkDaysInFullWeeks(days) + WorkDaysInPartialWeek(From.DayOfWeek, days);
            }
        }

        public override string ToString()
        {
            return $"{DateHelper2.ToDateId(From)}-{DateHelper2.ToDateId(To)}";
        }

        public IEnumerable<DatePeriod> SplitToIterations(int iterationDaysCount = 7)
        {
            var iterationFrom = From;
            DateTime iterationTo;
            do
            {
                var tempTo = iterationFrom.AddDays(iterationDaysCount - 1);
                iterationTo = tempTo >= To ? new DateTime(To.Year, To.Month, To.Day) : tempTo;
                yield return new DatePeriod(iterationFrom, iterationTo);
                iterationFrom = iterationTo.AddDays(1);
            } while (iterationTo < To);
        }

        private static int WorkDaysInFullWeeks(int totalDays)
        {
            return totalDays / 7 * 5;
        }

        private static int WorkDaysInPartialWeek(DayOfWeek firstDay, int totalDays)
        {
            var remainingDays = totalDays % 7;
            var daysToSaturday = (int) DayOfWeek.Saturday - (int) firstDay;
            if (remainingDays <= daysToSaturday)
                return remainingDays;
            /* daysToSaturday are the days before the weekend,
             * the rest of the expression computes the days remaining after we
             * ignore Saturday and Sunday
             */
            // Range ends in a Saturday or in a Sunday
            if (remainingDays <= daysToSaturday + 2)
                return daysToSaturday;
            // Range ends after a Sunday
            return remainingDays - 2;
        }
    }
    
    #region JsonConverter

    public class DatePeriodConverter : JsonConverter<DatePeriod>
    {
        public override void Write(Utf8JsonWriter writer, DatePeriod value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString());
            
        public override DatePeriod Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            return new DatePeriod(reader.GetString());
        }
    }

    #endregion
}