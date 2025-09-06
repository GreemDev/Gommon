using System;

// ReSharper disable MemberCanBePrivate.Global
namespace Gommon;

public static partial class Extensions
{
    public static string DateFormat { get; set; } = "MM/dd/yyyy";

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
    ///     Format the current <see cref="DateTimeOffset"/>'s date by the format specified in <see cref="DateFormat"/>.
    /// </summary>
    /// <param name="offset">Current <see cref="DateTimeOffset"/></param>
    /// <returns>Formatted <see cref="string"/></returns>
    public static string FormatDate(this DateTimeOffset offset)
        => offset.ToString(DateFormat);

    /// <summary>
    ///     Format the current <see cref="DateTime"/>'s date by the format specified in <see cref="DateFormat"/>.
    /// </summary>
    /// <param name="dt">Current <see cref="DateTime"/></param>
    /// <returns>Formatted <see cref="string"/></returns>
    public static string FormatDate(this DateTime dt)
        => dt.ToString(DateFormat);

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
    ///     Modify <see cref="Extensions"/>.<see cref="DateFormat"/> to change the date format.
    /// </summary>
    /// <param name="dt">Current <see cref="DateTime"/></param>
    /// <param name="usePartialTime">Whether or not to show hours in the time.</param>
    /// <returns></returns>
    public static string FormatPrettyString(this DateTime dt, bool usePartialTime = false)
        => $"at {(usePartialTime ? dt.FormatPartialTime() : dt.FormatFullTime())}, on {dt.FormatDate()}";

    /// <summary>
    ///     Formats the current <see cref="DateTimeOffset"/> to a pretty string such as "at 12:14:02 PM, on 04/16/2002"
    ///     Modify <see cref="Extensions"/>.<see cref="Extensions.DateFormat"/> to change the date format.
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
    /// <returns>A <see cref="ulong" /> representing the newly generated Snowflake ID</returns>
    public static ulong ToDiscordSnowflake(this DateTimeOffset offset)
        => (offset.ToUnixTimeMilliseconds().Cast<ulong>() - 1420070400000UL) << 22;

    public static string AsMarkdownString(this DateTime dt)
        => dt.FormatPrettyString().Split(" ").Apply(arr =>
        {
            arr[1] = embolden(arr[1]);
            arr[2] = embolden(arr[2].TrimEnd(',')).Append(',');
            arr[4] = embolden(arr[4]);
            return;

            string embolden(string s) => $"**{s}**";
        }).JoinToString(" ");

    public static string AsMarkdownString(this DateTimeOffset dt)
        => dt.DateTime.AsMarkdownString();

    /// <summary>
    ///     Transforms the current <see cref="UInt64"/> into a DateTimeOffset from a Discord snowflake.
    ///     If this Snowflake comes from an actual Discord entity, then it is the time it was created.
    ///     If this Snowflake is not from an actual Discord entity, then it's what that entity would have been created at provided it actually existed.
    /// </summary>
    /// <param name="id">The current <see cref="UInt64"/></param>
    /// <returns>A <see cref="DateTimeOffset"/> of when this Discord entity was created.</returns>
    public static DateTimeOffset AsDateTimeFromSnowflake(this ulong id)
        => DateTimeOffset.FromUnixTimeMilliseconds(((id >> 22) + 1420070400000).HardCast<long>());

    /// <summary>
    ///     Returns the current <see cref="DateTimeOffset"/> with time information removed. Offset is retained.
    /// </summary>
    public static DateTimeOffset WithoutTime(this DateTimeOffset dto)
        => new(dto.Year, dto.Month, dto.Day, 0, 0, 0, dto.Offset);

    /// <summary>
    ///     Returns a tuple with the current <see cref="DateTimeOffset"/> with time information removed, and the removed time.
    /// </summary>
    public static (DateTimeOffset Date, TimeSpan Time) Extract(this DateTimeOffset dto)
        => (dto.WithoutTime(), dto.TimeOfDay);

    /// <summary>
    ///     Returns the current <see cref="DateTime"/> with time information removed.
    /// </summary>
    public static DateTime WithoutTime(this DateTime dt)
        => new(dt.Year, dt.Month, dt.Day, 0, 0, 0);

    /// <summary>
    ///     Returns a tuple with the current <see cref="DateTime"/> with time information removed, and the removed time.
    /// </summary>
    public static (DateTime Date, TimeSpan Time) Extract(this DateTime dt)
        => (dt.WithoutTime(), dt.TimeOfDay);
}