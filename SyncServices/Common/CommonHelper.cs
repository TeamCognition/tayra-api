using System;

namespace Tayra.SyncServices.Common
{
    public static class CommonHelper
    {
        public static bool IsMonday(DateTime date) => date.DayOfWeek == DayOfWeek.Monday;
    }
}