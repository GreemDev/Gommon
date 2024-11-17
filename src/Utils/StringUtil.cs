using System;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

// ReSharper disable MemberCanBePrivate.Global
namespace Gommon;

public static class StringUtil {
    private const string AlphanumericChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static string RandomizeSequence([NotNull] string str, [NonNegativeValue] int rerolls = 0) {
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
    public static string RandomAlphanumeric([NonNegativeValue] int length, bool allowRepeats = true) {
        Guard.Ensure(length > 0, "length must be at least 1");
        var result = new StringBuilder(
            !allowRepeats && length > AlphanumericChars.Length 
                ? AlphanumericChars.Length
                : length
        );
        var tempChars = Collections.NewList(AlphanumericChars.ToArray());

        for (var i = 0; i < length; i++) {
            if (tempChars.None() && length > AlphanumericChars.Length)
                return result.ToString();

            var ch = tempChars.GetRandomElement();
            if (!allowRepeats)
                tempChars.Remove(ch);
            result.Append(ch);
        }

        return result.ToString();
    }
}