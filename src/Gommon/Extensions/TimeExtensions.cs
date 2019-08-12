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
        ///     Transforms the current <see cref="DateTimeOffset"/> to a Discord pseudo-snowflake.
        ///     Note that the resulting snowflake is not guaranteed to be tied to any Discord entity.
        /// </summary>
        /// <param name="offset">The current <see cref="DateTimeOffset"/></param>
        /// <returns> A <see cref="UInt64" /> representing the newly generated snowflake ID</returns>
        public static ulong ToDiscordSnowflake(this DateTimeOffset offset)
            => (offset.ToUnixTimeMilliseconds().Cast<ulong>() - 1420070400000UL) << 22;
    }
}
