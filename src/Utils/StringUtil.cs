using System;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace Gommon {
    public static class StringUtil {
        private const string _alphanumericChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static char[] ValidAlphanumerics => _alphanumericChars.ToArray();

        public static string RandomizeSequence([NotNull] string str, [NonNegativeValue] int rerolls = 0) {
            string _rearrange(string s) => string.Join("", s.OrderBy(_ => Guid.NewGuid()));

            var result = _rearrange(str);
            Lambda.Repeat(rerolls, () =>
                result = _rearrange(result)
            );
            return result;
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
            var result = new StringBuilder(length);
            var tempChars = Collections.NewList(_alphanumericChars.ToArray());

            for (var i = 0; i < length; i++) {
                if (tempChars.None() && length > _alphanumericChars.Length)
                    return result.ToString();

                var ch = tempChars.GetRandomElement();
                if (!allowRepeats)
                    tempChars.Remove(ch);
                result.Append(ch);
            }

            return result.ToString();
        }
    }
}