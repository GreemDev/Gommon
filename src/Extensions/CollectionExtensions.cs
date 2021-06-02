using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        ///     Automatically removes null entries, which shouldn't happen.
        /// </summary>
        /// <typeparam name="T">In type.</typeparam>
        /// <typeparam name="TKey">Type to check.</typeparam>
        /// <param name="coll">Current IEnumerable.</param>
        /// <param name="selector">Selector function.</param>
        /// <returns>The filtered <code>IEnumerable&lt;<typeparam name="T"/>&gt;</code></returns>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> coll, Func<T, TKey> selector)
            => coll.GroupBy(selector).Select(x => x.FirstOrDefault()).Where(x => x != null);

        /// <summary>
        ///     Join the current string Enumerable by the given string <paramref name="separator"/>.
        /// </summary>
        /// <param name="list">Current string Enumerable</param>
        /// <param name="separator">String separator</param>
        /// <returns><see cref="string"/> contents of the Enumerable, joined.</returns>
        public static string Join(this IEnumerable<string> list, string separator)
            => string.Join(separator, list);

        /// <summary>
        ///     Join the current string Enumerable by the given char <paramref name="separator"/>.
        /// </summary>
        /// <param name="list">Current string Enumerable</param>
        /// <param name="separator">Char separator</param>
        /// <returns><see cref="string"/> contents of the Enumerable, joined.</returns>
        public static string Join(this IEnumerable<string> list, char separator)
            => string.Join($"{separator}", list);

        /// <summary>
        ///     Get a random element in the current array.
        /// </summary>
        /// <param name="arr">Current array.</param>
        /// <returns>A random element in the current array.</returns>
        public static T GetRandomElement<T>(this T[] arr)
            => arr[new Random().Next(0, arr.Length)];
        
        /// <summary>
        ///     Get a random element in the current <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="enumerable">Current <see cref="IEnumerable{T}"/>.</param>
        /// <returns>A random element in the current <see cref="IEnumerable{T}"/>.</returns>
        public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
        {
            var arr = enumerable as T[] ?? enumerable.ToArray();
            return arr.ElementAt(new Random().Next(0, arr.Length));
        }


        /// <summary>
        ///     Performs the specified <paramref name="action"/> on each element in the current <paramref name="enumerable"/>; passing the current element as a parameter to the action.
        /// </summary>
        /// <param name="enumerable">The current enumerable.</param>
        /// <param name="action">The action to perform.</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable) action(item);
        }
        
        /// <summary>
        ///     Performs the specified <paramref name="function"/> on each element in the current <paramref name="enumerable"/> asynchronously; passing the current element as a parameter to the function.
        /// </summary>
        /// <param name="enumerable">The current enumerable.</param>
        /// <param name="function">The asynchronous function to perform.</param>
        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> function)
        {
            foreach (var item in enumerable) await function(item);
        }

        /// <summary>
        ///     Performs the specified <paramref name="action"/> on each element in the current <paramref name="enumerable"/>; passing the current element and index as a parameter to the action.
        /// </summary>
        /// <param name="enumerable">The current enumerable.</param>
        /// <param name="action">The action to perform.</param>
        public static void ForEachIndexed<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            var coll = enumerable.ToArray();
            for (var i = 0; i < coll.Length; i++)
                action(coll[i], i);
        }
        
        /// <summary>
        ///     Performs the specified <paramref name="function"/> on each element in the current <paramref name="enumerable"/> asynchronously; passing the current element and index as a parameter to the function.
        /// </summary>
        /// <param name="enumerable">The current enumerable.</param>
        /// <param name="function">The asynchronous function to perform.</param>
        public static async Task ForEachIndexedAsync<T>(this IEnumerable<T> enumerable, Func<T, int, Task> function)
        {
            var coll = enumerable.ToArray();
            for (var i = 0; i < coll.Length; i++)
                await function(coll[i], i);
        }

        /// <summary>
        ///     Converts the current IEnumerable to a human-readable string.
        /// </summary>
        /// <typeparam name="T">Type of the current Collection.</typeparam>
        /// <param name="coll">The current Enumerable.</param>
        /// <returns>A string representing the contents of the Collection.</returns>

        public static string ToReadableString<T>(this IEnumerable<T> coll) 
            => $"[{coll.Select(x => $"\"{x}\"").Join(", ")}]";

        /// <summary>
        ///     Checks whether or not the current IEnumerable is empty.
        /// </summary>
        /// <param name="coll">The current Enumerable.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>True if the current IEnumerable has any elements; false otherwise.</returns>

        public static bool IsEmpty<T>(this IEnumerable<T> coll) => !coll.Any();
    }
}

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static bool AnyGet<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate,
            out TSource value)
        {
            source = source.ToArray();
            if (source.Any(predicate))
            {
                value = source.FirstOrDefault(predicate);
                return true;
            }
            
            value = default;
            return false;
            
        }
    }
}
