using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;

namespace Gommon
{
    public static class PrimitiveExtensions
    {
        #region String

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


        #endregion

        #region Boolean

        [Obsolete("Use Humanzier's string#ToQuantity() method.")]
        public static bool ShouldBePlural(this int val) 
            => val != 1;

        #endregion

        #region Object

        /// <summary>
        ///     Casts the current object to the specified type. Returns that type's default if the current object is not of that type, usually null.
        /// </summary>
        /// <typeparam name="T">Type to cast to</typeparam>
        /// <param name="obj">Current object</param>
        /// <returns><paramref name="obj"/>, cast to the type of <typeparam name="T"/></returns>
        public static T Cast<T>(this object obj)
            => obj is T o ? o : default;

        #endregion
    }
}
