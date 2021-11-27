using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Gommon
{
    public static class Collections
    {
        //this might look dumb but it's basically to avoid new[]{ values } or new {CollectionType}<T>() { values }
        //you could also `using static` this class and call its methods without the class name, NewList(values)
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> NewList<T>([ItemCanBeNull] params T[] values) => new List<T>(values);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HashSet<T> NewHashSet<T>([ItemCanBeNull] params T[] values) => new HashSet<T>(values);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Queue<T> NewQueue<T>([ItemCanBeNull] params T[] initialValues) => new Queue<T>(initialValues);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LinkedList<T> NewLinkedList<T>([ItemCanBeNull] params T[] values) => new LinkedList<T>(values);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Stack<T> NewStack<T>([ItemCanBeNull] params T[] values) => new Stack<T>(values);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] NewArray<T>([ItemCanBeNull] params T[] values) => values;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<TKey, TValue> NewDictionary<TKey, TValue>(params (TKey Key, TValue Value)[] pairs)
            => pairs.ToDictionary(x => x.Key, x => x.Value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SafeDictionary<TKey, TValue> NewSafeDictionary<TKey, TValue>(
            params (TKey Key, TValue Value)[] pairs) where TValue : class
            => NewDictionary(pairs).AsSafe();

        public static SafeDictionary<TKey, TValue> AsSafe<TKey, TValue>([CanBeNull] this Dictionary<TKey, TValue> current)
            where TValue : class =>
            new SafeDictionary<TKey, TValue>(current ?? new Dictionary<TKey, TValue>());
    }
    
    /// <summary>
    ///     Mutable <see cref="Dictionary{TKey,TValue}"/> implementation that solely provides nullable indexing.
    /// </summary>
    /// <typeparam name="TKey">Entry key</typeparam>
    /// <typeparam name="TValue">Entry value</typeparam>
    public class SafeDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : class 
    {
        public SafeDictionary(Dictionary<TKey, TValue> initial = null)
            => (initial ?? new Dictionary<TKey, TValue>()).ForEach(x => Add(x.Key, x.Value));

        [CanBeNull] 
        public new TValue this[TKey key]
        {
            get => TryGetValue(key, out var value) ? value : null;
            set => base[key] = value;
        }
    }
}