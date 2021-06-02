using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Gommon
{
    public static partial class Extensions
    {
        #region String

        /// <summary>
        ///     Repeats <paramref name="str"/> <paramref name="times"/> times.
        /// </summary>
        /// <param name="str">Current string</param>
        /// <param name="times">The amount of times the string should be repeated.</param>
        /// <returns>The resulting string.</returns>
        public static string Repeat(this string str, int times) 
            => new StringBuilder().Apply(sb => Lambda.Repeat(times, () => sb.Append(str))).ToString();
        
        /// <summary>
        ///     Prepends <paramref name="other"/> to the beginning of <paramref name="str"/> and returns it.
        /// </summary>
        /// <param name="str">Current string</param>
        /// <param name="other">The value to prepend</param>
        /// <returns><paramref name="str"/> with <paramref name="other"/> prepended to it.</returns>
        public static string Prepend(this string str, string other) => str.Insert(0, other);
        
        /// <summary>
        ///     Checks whether or not the current string contains any of <paramref name="potentialMatches"/>, ignoring case.
        /// </summary>
        /// <param name="str">Current string</param>
        /// <param name="potentialMatches">Strings to try and match</param>
        /// <returns><see cref="bool"/></returns>
        public static bool EqualsAnyIgnoreCase(this string str, params string[] potentialMatches) 
            => potentialMatches.Any(str.EqualsIgnoreCase);

        /// <summary>
        ///     Checks whether or not <paramref name="otherString"/> is equal to the current string, ignoring case.
        /// </summary>
        /// <param name="str">Current string</param>
        /// <param name="otherString">String to compare</param>
        /// <returns><see cref="bool"/></returns>
        public static bool EqualsIgnoreCase(this string str, string otherString) 
            => !(str is null) && str.Equals(otherString, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        ///     Checks whether or not <paramref name="value"/> is included in the current string, ignoring case.
        /// </summary>
        /// <param name="str">Current string</param>
        /// <param name="value">String to compare</param>
        /// <returns><see cref="bool"/></returns>
        public static bool ContainsIgnoreCase(this string str, string value)
            => !(str is null) && str.ToLower().Contains(value.ToLower());

        /// <summary>
        ///     Checks whether or not the current string is null or entirely whitespace.
        /// </summary>
        /// <param name="str">Current string</param>
        /// <returns><see cref="bool"/></returns>
        public static bool IsNullOrWhitespace(this string str) 
            => string.IsNullOrWhiteSpace(str);

        /// <summary>
        ///     Checks whether or not the current string is null or completely empty.
        /// </summary>
        /// <param name="str">Current string</param>
        /// <returns><see cref="bool"/></returns>
        public static bool IsNullOrEmpty(this string str) 
            => string.IsNullOrEmpty(str);

        /// <summary>
        ///     Gets the current string's unicode points. Typically for unicode emojis.
        /// </summary>
        /// <param name="str">Current string</param>
        /// <returns>Enumerable of Unicode code points</returns>
        public static IEnumerable<int> GetUnicodePoints(this string str)
        {
            var pts = new List<int>(str.Length);
            for (var i = 0; i < str.Length; i++)
            {
                var pt = char.ConvertToUtf32(str, i);
                if (pt != 0xFE0F)
                    pts.Add(pt);
                if (char.IsHighSurrogate(str[i]))
                    i++;
            }

            return pts;
        }

        /// <summary>
        ///     Returns whether or not the current string starts with another string, ignoring case.
        /// </summary>
        /// <param name="str">Current string</param>
        /// <param name="otherString">String to compare</param>
        /// <returns><see cref="bool"/></returns>
        public static bool StartsWithIgnoreCase(this string str, string otherString) 
            => !(str is null) && str.StartsWith(otherString, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        ///     Returns whether or not the current string ends with another string, ignoring case.
        /// </summary>
        /// <param name="str">Current string</param>
        /// <param name="otherString">String to compare</param>
        /// <returns><see cref="bool"/></returns>
        public static bool EndsWithIgnoreCase(this string str, string otherString)
            => !(str is null) && str.EndsWith(otherString, StringComparison.OrdinalIgnoreCase);


        /// <summary>
        ///     Reverses the content of the current string.
        ///
        ///     Example:
        ///         "Hello!" => "!olleH"
        /// </summary>
        /// <example>
        ///     Input: "Hello!"
        ///     Output: "!olleH"
        /// </example>
        /// <param name="str">Current string</param>
        /// <returns>The current string, but reversed.</returns>
        public static string Reverse(this string str)
        {
            var arr = str.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }


        #endregion

        #region Object

        /// <summary>
        ///     Casts the current object to the specified type. Returns that type's default if the current object is not of that type, usually null.
        ///     This extension never throws an exception.
        /// </summary>
        /// <typeparam name="T">Type to cast to</typeparam>
        /// <param name="obj">Current object</param>
        /// <returns><paramref name="obj"/>, cast to the type of <typeparamref name="T"/></returns>
        public static T Cast<T>(this object obj)
            => obj is T o ? o : default;


        /// <summary>
        ///     "Hard" casts the current object to the specified type. Throws an <see cref="InvalidCastException"/> if the current object is not assignable to that type,
        /// so <see cref="Cast{T}"/> might be preferable.
        /// </summary>
        /// <typeparam name="T">Type to cast to</typeparam>
        /// <param name="obj">Current object</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <returns><paramref name="obj"/>, cast to the type of <typeparamref name="T"/>, or an exception is thrown.</returns>
        public static T HardCast<T>(this object obj)
            => (T)obj;

        #endregion
    }
}
