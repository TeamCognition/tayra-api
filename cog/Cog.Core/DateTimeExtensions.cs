using System;

namespace Cog.Core
{
    public static class DateTimeExtensions
    {
        public static DateTime ConvertUnixEpochTime(long milliseconds)
        {
            var Fecha = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Fecha.AddMilliseconds(milliseconds);
        }
    }
}