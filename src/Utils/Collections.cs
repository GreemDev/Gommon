using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

// ReSharper disable MemberCanBePrivate.Global
namespace Gommon;

public static class Collections
{
    //this might look dumb, but it's basically to avoid new[]{ values } or new {CollectionType}<T>() { values }
    //you could also `using static` this class and call its methods without the class name, NewList(values)

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static List<T> NewList<T>(params T[] values) => [..values];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashSet<T> NewHashSet<T>(params T[] values) => [..values];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Queue<T> NewQueue<T>(params T[] initialValues) => new(initialValues);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LinkedList<T> NewLinkedList<T>(params T[] values) => new(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Stack<T> NewStack<T>(params T[] values) => new(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] NewArray<T>(params T[] values) => values;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary<TKey, TValue> NewDictionary<TKey, TValue>(params (TKey Key, TValue Value)[] pairs)
        => pairs.ToDictionary(x => x.Key, x => x.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SafeDictionary<TKey, TValue> NewSafeDictionary<TKey, TValue>(
        params (TKey Key, TValue Value)[] pairs) where TValue : class
        => NewDictionary(pairs).AsSafe();

    public static SafeDictionary<TKey, TValue> AsSafe<TKey, TValue>(this Dictionary<TKey, TValue> current)
        where TValue : class
        => new(current ?? new Dictionary<TKey, TValue>());
}

/// <summary>
///     Mutable alternative <see cref="Dictionary{TKey,TValue}"/> implementation solely for nullable indexing.
/// </summary>
/// <typeparam name="TKey">Entry key</typeparam>
/// <typeparam name="TValue">Entry value</typeparam>
public class SafeDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : class
{
    public SafeDictionary(Dictionary<TKey, TValue> initial) : base(initial.Count)
        => initial.ForEach(x => Add(x.Key, x.Value));

    public SafeDictionary()
    {
    }

    [MaybeNull]
    public new TValue this[TKey key]
    {
        get => this.FindValue(key).OrDefault();
        set => base[key] = value;
    }
}