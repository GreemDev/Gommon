using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Gommon {
    public static class Possibles {
        public static PossibleRef<TValue> FromStruct<TValue>(TValue? value) where TValue : struct
            => new PossibleRef<TValue>(value);
        
        public static Possible<TValue> From<TValue>([CanBeNull] TValue value) => new Possible<TValue>(value);
        
        /// <summary>
        ///     Wraps the result of LINQ First() in a <see cref="PossibleRef{TValue}"/>.
        /// </summary>
        /// <param name="coll">The current collection.</param>
        /// <param name="predicate">The predicate function used to find the first matching element.</param>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <returns>An optional, possibly containing the value that matches the predicate.</returns>
        public static PossibleRef<T> FindFirstRef<T>(this IEnumerable<T> coll, Func<T, bool> predicate = null)
            where T : struct
            => coll.FirstOrDefault(v => predicate is null || predicate(v));

        /// <summary>
        ///     Wraps the result of LINQ Last() in a <see cref="PossibleRef{TValue}"/>.
        /// </summary>
        /// <param name="coll">The current collection.</param>
        /// <param name="predicate">The predicate function used to find the last matching element.</param>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <returns>An optional, possibly containing the value that matches the predicate.</returns>
        public static PossibleRef<T> FindLastRef<T>(this IEnumerable<T> coll, Func<T, bool> predicate = null)
            where T : struct
            => coll.LastOrDefault(v => predicate is null || predicate(v));

        /// <summary>
        ///     Wraps the result of LINQ Single() in a <see cref="PossibleRef{TValue}"/>.
        /// </summary>
        /// <param name="coll">The current collection.</param>
        /// <param name="predicate">The predicate function used to find the element.</param>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <returns>An optional, possibly containing the value that matches the predicate.</returns>
        public static PossibleRef<T> FindSingleRef<T>(this IEnumerable<T> coll, Func<T, bool> predicate = null)
            where T : struct
            => coll.SingleOrDefault(v => predicate is null || predicate(v));
        
        /// <summary>
        ///     Wraps the result of LINQ ElementAt() in a <see cref="PossibleRef{TValue}"/>.
        /// </summary>
        /// <param name="coll">The current collection.</param>
        /// <param name="index">The index of the element.</param>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <returns>An optional, possibly containing the value at the specified index.</returns>
        public static PossibleRef<T> FindElementRef<T>(this IEnumerable<T> coll, int index)
            where T : struct
            => coll.ElementAtOrDefault(index);

        /// <summary>
        ///     Wraps the result of a GetRandomElement() in a <see cref="PossibleRef{TValue}"/>.
        /// </summary>
        /// <param name="coll">The current collection.</param>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <returns>An optional, possibly containing the value that matches the predicate.</returns>
        public static PossibleRef<T> FindRandomElementRef<T>(this IEnumerable<T> coll)
            where T : struct
            => coll.GetRandomElement();

        /// <summary>
        ///     Wraps <i>any</i> value, null or otherwise (with no <see cref="NullReferenceException"/>s), in an <see cref="PossibleRef{TValue}"/>.
        /// </summary>
        /// <param name="obj">The current object.</param>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <returns>An optional, containing the possibly-null value of <paramref name="obj"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PossibleRef<T> AsPossibleRef<T>(this T obj) where T : struct => new PossibleRef<T>(obj);
        
        /// <summary>
        ///     Wraps the result of LINQ First() in a <see cref="Possible{TValue}"/>.
        /// </summary>
        /// <param name="coll">The current collection.</param>
        /// <param name="predicate">The predicate function used to find the first matching element.</param>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <returns>An optional, possibly containing the value that matches the predicate.</returns>
        public static Possible<T> FindFirst<T>(this IEnumerable<T> coll, Func<T, bool> predicate = null)
            => coll.FirstOrDefault(v => predicate is null || predicate(v));

        /// <summary>
        ///     Wraps the result of LINQ Last() in a <see cref="Possible{TValue}"/>.
        /// </summary>
        /// <param name="coll">The current collection.</param>
        /// <param name="predicate">The predicate function used to find the last matching element.</param>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <returns>An optional, possibly containing the value that matches the predicate.</returns>
        public static Possible<T> FindLast<T>(this IEnumerable<T> coll, Func<T, bool> predicate = null)
            => coll.LastOrDefault(v => predicate is null || predicate(v));

        /// <summary>
        ///     Wraps the result of LINQ Single() in a <see cref="Possible{TValue}"/>.
        /// </summary>
        /// <param name="coll">The current collection.</param>
        /// <param name="predicate">The predicate function used to find the element.</param>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <returns>An optional, possibly containing the value that matches the predicate.</returns>
        public static Possible<T> FindSingle<T>(this IEnumerable<T> coll, Func<T, bool> predicate = null)
            => coll.SingleOrDefault(v => predicate is null || predicate(v));
        
        /// <summary>
        ///     Wraps the result of LINQ ElementAt() in a <see cref="Possible{TValue}"/>.
        /// </summary>
        /// <param name="coll">The current collection.</param>
        /// <param name="index">The index of the element.</param>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <returns>An optional, possibly containing the value at the specified index.</returns>
        public static Possible<T> FindElement<T>(this IEnumerable<T> coll, int index) 
            => coll.ElementAtOrDefault(index);

        /// <summary>
        ///     Wraps the result of a GetRandomElement() in a <see cref="Possible{TValue}"/>.
        /// </summary>
        /// <param name="coll">The current collection.</param>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <returns>An optional, possibly containing the value that matches the predicate.</returns>
        public static Possible<T> FindRandomElement<T>(this IEnumerable<T> coll)
            => coll.GetRandomElement();

        /// <summary>
        ///     Wraps <i>any</i> value, null or otherwise (with no <see cref="NullReferenceException"/>s), in an <see cref="Possible{TValue}"/>.
        /// </summary>
        /// <param name="obj">The current object.</param>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <returns>An optional, containing the possibly-null value of <paramref name="obj"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Possible<T> AsPossible<T>(this T obj) => new Possible<T>(obj);
    }
}