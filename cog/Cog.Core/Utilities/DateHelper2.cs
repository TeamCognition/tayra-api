using System;
using System.Linq;
using System.Threading;

namespace Cog.Core
{
    public static class DateHelper2
    {
        #region Constants

        public const string DATE_FORMAT = "yyyyMMdd";
        public const string DATE_TIME_FORMAT = "yyyyMMddHHmmss";

        #endregion

        #region Properties

        public static long Today => long.Parse(DateTime.UtcNow.ToString(DATE_FORMAT));

        public static long Now => long.Parse(DateTime.UtcNow.ToString(DATE_TIME_FORMAT));

        #endregion

        #region Public Methods

        public static int ToDateId(DateTime date)
        {
            return int.Parse(ToDateString(date));
        }

        public static long ToDateTimeId(DateTime date)
        {
            return long.Parse(ToDateTimeString(date));
        }

        public static string ToDateString(DateTime date)
        {
            return date.ToString(DATE_FORMAT);
        }

        public static string ToDateTimeString(DateTime date)
        {
            return date.ToString(DATE_TIME_FORMAT);
        }

        public static DateTime ParseDate(string dateString)
        {
            return DateTime.ParseExact(dateString, DATE_FORMAT, Thread.CurrentThread.CurrentCulture);
        }

        public static DateTime ParseDate(long dateId)
        {
            return ParseDate(dateId.ToString());
        }

        public static DateTime ParseDateTime(string dateString)
        {
            return DateTime.ParseExact(dateString, DATE_TIME_FORMAT, Thread.CurrentThread.CurrentCulture);
        }

        public static DateTime ParseDateTime(long dateId)
        {
            return ParseDateTime(dateId.ToString());
        }

        public static string ToDayOfWeekString(int dateId)
        {
            return ParseDate(dateId).DayOfWeek.ToString();
        }

        public static int ToMonthIndex(int dateId)
        {
            return ParseDate(dateId).Month;
        }

        public static string[] CreateDayOfWeekList(int fromDateId, int periodInDays)
        {
            var startDate = ParseDate(fromDateId).AddDays(-periodInDays + 1);
            return Enumerable.Range(0, periodInDays).Select(x => startDate.AddDays(x).DayOfWeek.ToString()).ToArray();
        }

        public static int AddDays(int dateId, int days)
        {
            return ToDateId(ParseDate(dateId).AddDays(days));
        }

        public static long GetCurrentUnixTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        #endregion
    }
}