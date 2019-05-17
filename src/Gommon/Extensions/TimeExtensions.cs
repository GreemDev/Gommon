using System;

namespace Gommon
{
    public static class TimeExtensions
    {
        /// <summary>
        ///     Format the current <see cref="DateTimeOffset"/>'s time by the <code>"hh:mm:ss tt"</code> format.
        /// </summary>
        /// <param name="offset">Current <see cref="DateTimeOffset"/></param>
        /// <returns>Formatted <see cref="string"/></returns>
        public static string FormatFullTime(this DateTimeOffset offset)
            => offset.ToString("hh:mm:ss tt");

        /// <summary>
        ///     Format the current <see cref="DateTimeOffset"/>'s time by the <code>"mm:ss tt"</code> format.
        /// </summary>
        /// <param name="offset">Current <see cref="DateTimeOffset"/></param>
        /// <returns>Formatted <see cref="string"/></returns>
        public static string FormatPartialTime(this DateTimeOffset offset)
            => offset.ToString("mm:ss tt");

        /// <summary>
        ///     Format the current <see cref="DateTimeOffset"/>'s date by the <code>"MM/dd/yyyy"</code> format.
        /// </summary>
        /// <param name="offset">Current <see cref="DateTimeOffset"/></param>
        /// <returns>Formatted <see cref="string"/></returns>
        public static string FormatDate(this DateTimeOffset offset)
            => offset.ToString("MM/dd/yyyy");
    }
}
