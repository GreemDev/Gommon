using System;

namespace Gommon
{
    public static partial class Extensions
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

        /// <summary>
        ///     Format the current <see cref="DateTime"/>'s date by the <code>"MM/dd/yyyy"</code> format.
        /// </summary>
        /// <param name="dt">Current <see cref="DateTime"/></param>
        /// <returns>Formatted <see cref="string"/></returns>
        public static string FormatDate(this DateTime dt)
            => dt.ToString("MM/dd/yyyy");

        /// <summary>
        ///     Format the current <see cref="DateTime"/>'s time by the <code>"hh:mm:ss tt"</code> format.
        /// </summary>
        /// <param name="dt">Current <see cref="DateTime"/></param>
        /// <returns>Formatted <see cref="string"/></returns>
        public static string FormatFullTime(this DateTime dt)
            => dt.ToString("hh:mm:ss tt");

        /// <summary>
        ///     Format the current <see cref="DateTime"/>'s time by the <code>"mm:ss tt"</code> format.
        /// </summary>
        /// <param name="dt">Current <see cref="DateTime"/></param>
        /// <returns>Formatted <see cref="string"/></returns>
        public static string FormatPartialTime(this DateTime dt)
            => dt.ToString("mm:ss tt");

        /// <summary>
        ///     Formats the current <see cref="DateTime"/> to a pretty string such as "at 12:14:02 PM, on 04/16/2002"
        ///     Uses the American date format, "MM/dd/yyyy".
        /// </summary>
        /// <param name="dt">Current <see cref="DateTime"/></param>
        /// <param name="usePartialTime">Whether or not to show hours in the time.</param>
        /// <returns></returns>
        public static string FormatPrettyString(this DateTime dt, bool usePartialTime = false)
            => $"at {(usePartialTime ? dt.FormatPartialTime() : dt.FormatFullTime())}, on {dt.FormatDate()}";

        /// <summary>
        ///     Formats the current <see cref="DateTimeOffset"/> to a pretty string such as "at 12:14:02 PM, on 04/16/2002"
        ///     Uses the American date format, "MM/dd/yyyy".
        /// </summary>
        /// <param name="offset">Current <see cref="DateTimeOffset"/></param>
        /// <param name="usePartialTime">Whether or not to show hours in the time.</param>
        /// <returns></returns>
        public static string FormatPrettyString(this DateTimeOffset offset, bool usePartialTime = false)
            => $"at {(usePartialTime ? offset.FormatPartialTime() : offset.FormatFullTime())}, on {offset.FormatDate()}";

        /// <summary>
        ///     Transforms the current <see cref="DateTimeOffset"/> to a Discord pseudo-snowflake.
        ///     Note that the resulting snowflake is not guaranteed to be tied to any Discord entity.
        /// </summary>
        /// <param name="offset">The current <see cref="DateTimeOffset"/></param>
        /// <returns> A <see cref="ulong" /> representing the newly generated Snowflake ID</returns>
        public static ulong ToDiscordSnowflake(this DateTimeOffset offset)
            => (offset.ToUnixTimeMilliseconds().Cast<ulong>() - 1420070400000UL) << 22;
    }
}
