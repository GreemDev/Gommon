using System;

namespace Gommon
{
    public static class TimeExtensions
    {
        public static string FormatFullTime(this DateTimeOffset offset)
            => offset.ToString("hh:mm:ss tt");

        public static string FormatPartialTime(this DateTimeOffset offset)
            => offset.ToString("mm:ss tt");

        public static string FormatDate(this DateTimeOffset offset)
            => offset.ToString("MM/dd/yyyy");
    }
}
