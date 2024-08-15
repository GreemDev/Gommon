using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Gommon;

public static partial class Extensions
{
    /// <summary>
    ///     Run the specified <see cref="Func{TResult}"/> with <paramref name="curr"/> passed in as its value,
    ///     then return <paramref name="curr"/>.
    /// </summary>
    /// <param name="curr">The current object.</param>
    /// <param name="applyAsync">The async action to perform.</param>
    /// <typeparam name="T">The type of the current object.</typeparam>
    /// <returns><paramref name="curr"/> after <paramref name="applyAsync"/> runs on it.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<T> ApplyAsync<T>(this T curr, Func<T, Task> applyAsync)
    {
        await applyAsync(curr).ConfigureAwait(false);
        return curr;
    }

    /// <summary>
    ///     Run the specified <see cref="Func{T, TResult}"/> with <paramref name="curr"/> passed in as its value,
    ///     then return nothing.
    /// </summary>
    /// <param name="curr">The current object.</param>
    /// <param name="applyAsync">The async action to perform.</param>
    /// <typeparam name="T">The type of the current object.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task RunAsync<T>(this T curr, Func<T, Task> applyAsync)
        => await applyAsync(curr).ConfigureAwait(false);

    /// <summary>
    ///     Run the specified <see cref="Func{T, TResult}"/> with <paramref name="curr"/> passed in as its value,
    ///     then return the result of the delegate.
    /// </summary>
    /// <param name="curr">The current object.</param>
    /// <param name="applyAsync">The async action to perform.</param>
    /// <typeparam name="T">The type of the current object.</typeparam>
    /// <typeparam name="TResult">The type that is returned by the delegate '<paramref name="applyAsync"/>'.</typeparam>
    /// <returns>The result of running '<paramref name="applyAsync"/>' on '<paramref name="curr"/>'.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<TResult> IntoAsync<T, TResult>(this T curr, Func<T, Task<TResult>> applyAsync)
        => await applyAsync(curr).ConfigureAwait(false);

    /// <summary>
    ///     Run the specified <see cref="Action{T}"/> with <paramref name="curr"/> passed in as its value,
    ///     then return <paramref name="curr"/>.
    /// </summary>
    /// <param name="curr">The current object.</param>
    /// <param name="apply">The action to perform.</param>
    /// <typeparam name="T">The type of the current object.</typeparam>
    /// <returns><paramref name="curr"/> after <paramref name="apply"/> runs on it.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Apply<T>(this T curr, Action<T> apply)
    {
        apply(curr);
        return curr;
    }

    /// <summary>
    ///     Run the specified <see cref="Action{T}"/> with <paramref name="curr"/> passed in as its value,
    ///     then return nothing.
    /// </summary>
    /// <param name="curr">The current object.</param>
    /// <param name="apply">The action to perform.</param>
    /// <typeparam name="T">The type of the current object.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Run<T>(this T curr, Action<T> apply) => apply(curr);

    /// <summary>
    ///     Run the specified <see cref="Func{T, TResult}"/> with <paramref name="curr"/> passed in as its value,
    ///     then return the result of the delegate.
    /// </summary>
    /// <param name="curr">The current object.</param>
    /// <param name="apply">The action to perform.</param>
    /// <typeparam name="TIn">The type of the current object.</typeparam>
    /// <typeparam name="TOut">The type that is returned by the delegate '<paramref name="apply"/>'.</typeparam>
    /// <returns>The result of running '<paramref name="apply"/>' on '<paramref name="curr"/>'.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TOut Into<TIn, TOut>(this TIn curr, Func<TIn, TOut> apply)
        => apply(curr);
}