﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gommon.Extensions
{
    public static class CollectionExtensions
    {
        public static Stream ToStream(this IEnumerable<byte> bytes)
            => new MemoryStream(bytes.Cast<byte[]>() ?? bytes.ToArray(), false) { Position = 0 };

        public static bool ContainsIgnoreCase(this IEnumerable<string> strings, string element)
            => strings.Contains(element, StringComparer.OrdinalIgnoreCase);

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> coll, Func<T, TKey> selector)
            => coll.GroupBy(selector).Select(x => x.FirstOrDefault());

        public static string Join(this IEnumerable<string> list, string separator)
            => string.Join(separator, list);

        public static string Join(this IEnumerable<string> list, char separator)
            => string.Join($"{separator}", list);

        public static string Random(this string[] arr)
            => arr[new Random().Next(0, arr.Length)];
    }
}
