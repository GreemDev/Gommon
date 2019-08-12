using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gommon
{
    public static partial class Extensions
    {
        /// <summary>
        ///     Converts the specified <code>IEnumerable&lt;byte&gt;</code> to a <see cref="MemoryStream"/>.
        /// </summary>
        /// <param name="bytes">Byte enumerable to convert.</param>
        /// <returns>The resulting <see cref="MemoryStream"/>, seek-ed to position 0.</returns>
        public static MemoryStream ToStream(this IEnumerable<byte> bytes)
            => new MemoryStream(bytes.Cast<byte[]>() ?? bytes.ToArray(), false) { Position = 0 };

        /// <summary>
        ///     Checks whether or not the current <code>IEnumerable&lt;string&gt;</code> contains the specified <paramref name="element"/>, ignoring case.
        /// </summary>
        /// <param name="strings">String enumerable to check.</param>
        /// <param name="element">Element to check for.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool ContainsIgnoreCase(this IEnumerable<string> strings, string element)
            => strings.Contains(element, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     Effectively "filters" the current IEnumerable by removing duplicates by the return type of the <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="T">In type.</typeparam>
        /// <typeparam name="TKey">Type to check.</typeparam>
        /// <param name="coll">Current IEnumerable.</param>
        /// <param name="selector">Selector function.</param>
        /// <returns>The filtered <code>IEnumerable&lt;<typeparam name="T"/>&gt;</code></returns>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> coll, Func<T, TKey> selector)
            => coll.GroupBy(selector).Select(x => x.FirstOrDefault());

        /// <summary>
        ///     Join the current Enumerable by the given string <paramref name="separator"/>.
        /// </summary>
        /// <param name="list">Current Enumerable</param>
        /// <param name="separator">String separator</param>
        /// <returns><see cref="string"/> contents of the Enumerable, joined.</returns>
        public static string Join(this IEnumerable<string> list, string separator)
            => string.Join(separator, list);

        /// <summary>
        ///     Join the current Enumerable by the given char <paramref name="separator"/>.
        /// </summary>
        /// <param name="list">Current Enumerable</param>
        /// <param name="separator">Char separator</param>
        /// <returns><see cref="string"/> contents of the Enumerable, joined.</returns>
        public static string Join(this IEnumerable<string> list, char separator)
            => string.Join($"{separator}", list);

        /// <summary>
        ///     Get a random element in the current array.
        /// </summary>
        /// <param name="arr">Current array.</param>
        /// <returns>Random element in the current array.</returns>
        public static object Random(this object[] arr)
            => arr[new Random().Next(0, arr.Length)];
    }
}
