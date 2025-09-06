using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

// ReSharper disable MemberCanBePrivate.Global
namespace Gommon;

public static class StringUtil
{
    private const string AlphanumericChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static string RandomizeSequence([NotNull] string str, int rerolls = 0)
    {
        var result = rearrange(str);
        Lambda.Repeat(rerolls, () =>
            result = rearrange(result)
        );
        return result;

        string rearrange(string s)
            => s.OrderBy(_ => Guid.NewGuid())
                .JoinToString("");
    }

    /// <summary>
    ///     Creates a randomized alphanumeric string. <br/>
    ///     If <paramref name="allowRepeats"/> is true, characters *can*
    ///     be repeated, and the result will always be the length of <see cref="length"/>. <br/>
    ///     If <paramref name="length"/> is greater than 62, and <paramref name="allowRepeats"/> is false, the string
    ///     will only be 62 characters in length, as there is only that many characters to choose from without duplicating.
    /// </summary>
    /// <param name="length"></param>
    /// <param name="allowRepeats"></param>
    /// <returns></returns>
    public static string RandomAlphanumeric(int length, bool allowRepeats = true)
    {
        Guard.Ensure(length > 0, "length must be at least 1");
        var result = new StringBuilder(
            !allowRepeats && length > AlphanumericChars.Length
                ? AlphanumericChars.Length
                : length
        );
        var tempChars = Collections.NewList(AlphanumericChars.ToArray());

        for (var i = 0; i < length; i++)
        {
            if (tempChars.None() && length > AlphanumericChars.Length)
                return result.ToString();

            var ch = tempChars.GetRandomElement();
            if (!allowRepeats)
                tempChars.Remove(ch);
            result.Append(ch);
        }

        return result.ToString();
    }

    /// <summary>
    ///     Formats an arbitrary collection into a <see cref="string"/>.
    /// </summary>
    /// <param name="enumerable">The collection to format.</param>
    /// <param name="toString">The function applied to every element that stringifies them.</param>
    /// <param name="separator">The separator in-between elements in the resulting <see cref="string"/>.</param>
    /// <param name="prefix">The prefix of the resulting <see cref="string"/>.</param>
    /// <param name="suffix">The suffix of the resulting <see cref="string"/>.</param>
    /// <param name="emptyCollectionFallback">The value returned if the <see cref="IEnumerable{T}"/> is empty.</param>
    /// <typeparam name="T">The type of elements in the <see cref="IEnumerable{T}"/>.</typeparam>
    /// <returns>A sanely-formatted collection string.</returns>
    public static string FormatCollection<T>(
        this IEnumerable<T> enumerable,
        Func<T, string> toString,
        string separator = ",",
        string prefix = "",
        string suffix = "",
        string emptyCollectionFallback = "None"
    )
    {
        var coll = enumerable.ToArray();
        return coll.Length == 0
            ? emptyCollectionFallback
            : $"{prefix}{coll.Select(toString).JoinToString(separator)}{suffix}";
    }
}