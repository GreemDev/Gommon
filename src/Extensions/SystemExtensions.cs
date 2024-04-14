using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

// ReSharper disable MemberCanBePrivate.Global
namespace Gommon {
    public partial class Extensions {
        /// <summary>
        ///     Gets a service of the type <typeparamref name="T"/>.
        ///     If the service isn't present, this will return <see langword="null"/>.
        /// </summary>
        /// <param name="provider">The <see cref="IServiceProvider"/>.</param>
        /// <typeparam name="T">The type of Service to retrieve.</typeparam>
        /// <returns>A service of type <typeparamref name="T"/>; null if not present.</returns>
        public static T Get<T>(this IServiceProvider provider)
            => provider.GetService(typeof(T)).Cast<T>();

        /// <summary>
        ///     Tries to retrieve a service from the <paramref name="serviceProvider"/>.
        ///     If this method returns true, <paramref name="service"/> will not be null.
        ///     If this method returns false, <paramref name="service"/> will always be null.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/>.</param>
        /// <param name="service">The retrieved service of type <typeparamref name="T"/>.</param>
        /// <typeparam name="T">The type of service to retrieve.</typeparam>
        /// <returns></returns>
        public static bool TryGet<T>(this IServiceProvider serviceProvider, out T service) {
            service = serviceProvider.Get<T>();
            return service != null;
        }

        /// <summary>
        ///     Takes the current Semaphore's state and wraps it in a disposable, for use in using statements.
        /// </summary>
        /// <param name="semaphore">The semaphore</param>
        /// <returns>A disposable Semaphore lock</returns>
        public static ScopedSemaphoreLock Take(this Semaphore semaphore) {
            semaphore.WaitOne();
            return new ScopedSemaphoreLock(semaphore);
        }

        /// <summary>
        ///     Takes the current Semaphore's state and wraps it in a disposable, for use in using statements.
        /// </summary>
        /// <param name="semaphore">The semaphore</param>
        /// <returns>A disposable Semaphore lock</returns>
        public static async Task<ScopedSemaphoreLock> TakeAsync(this SemaphoreSlim semaphore) {
            await semaphore.WaitAsync();
            return new ScopedSemaphoreLock(semaphore);
        }

        public class ScopedSemaphoreLock : IDisposable {
            public ScopedSemaphoreLock(SemaphoreSlim semaphore) => _semaphoreSlim = semaphore;
            public ScopedSemaphoreLock(Semaphore semaphore) => _semaphore = semaphore;
            private readonly SemaphoreSlim _semaphoreSlim;
            private readonly Semaphore _semaphore;

            public void Dispose() {
                _semaphore?.Release();
                _semaphoreSlim?.Release();
            }
        }

        /// <summary>
        ///     Run the specified <see cref="Func{T, TResult}"/> with <paramref name="curr"/> passed in as its value.
        /// </summary>
        /// <param name="curr">The current object.</param>
        /// <param name="applyAsync">The async action to perform.</param>
        /// <typeparam name="T">The type of the current object.</typeparam>
        /// <returns><paramref name="curr"/> after <paramref name="applyAsync"/> runs on it.</returns>
        public static async Task<T> ApplyAsync<T>(this T curr, Func<T, Task> applyAsync) {
            await applyAsync(curr);
            return curr;
        }

        /// <summary>
        ///     Run the specified <see cref="Action{T}"/> with <paramref name="curr"/> passed in as its value.
        /// </summary>
        /// <param name="curr">The current object.</param>
        /// <param name="apply">The action to perform.</param>
        /// <typeparam name="T">The type of the current object.</typeparam>
        /// <returns><paramref name="curr"/> after <paramref name="apply"/> runs on it.</returns>
        public static T Apply<T>(this T curr, Action<T> apply) {
            apply(curr);
            return curr;
        }

        public static T ValueLock<T>(this object @lock, Func<T> action) {
            lock (@lock)
                return action();
        }

        public static void Lock(this object @lock, Action action) {
            lock (@lock)
                action();
        }

        public static void LockedRef<T>(this T obj, Action<T> action) {
            lock (obj)
                action(obj);
        }

        private const int _memoryTierSize = 1024;

        /// <summary>
        ///     Gets all flags in the current enum.
        /// </summary>
        /// <param name="input">The current enum.</param>
        /// <typeparam name="T">The enum's type.</typeparam>
        /// <returns>A collection of all the current Enum's members.</returns>
        public static IEnumerable<T> GetFlags<T>([NotNull] this T input) where T : Enum
            => Enumerable.Cast<T>(Enum.GetValues(input.GetType())).Where(e => input.HasFlag(e));


        /// <summary>
        ///     Appends all elements in the specified string array as a line to the current StringBuilder.
        /// </summary>
        /// <param name="sb">The current StringBuilder.</param>
        /// <param name="lines">The lines to append.</param>
        /// <returns>The current StringBuilder for chaining convenience.</returns>
        public static StringBuilder AppendAllLines(this StringBuilder sb, params string[] lines) {
            lines.ForEach(l => sb.AppendLine(l));
            return sb;
        }

        /// <summary>
        ///     Attempts to match a string against regex and the resulting match is the out variable.
        /// </summary>
        /// <param name="regex">The current regex.</param>
        /// <param name="str">The string to attempt to match to.</param>
        /// <param name="match">The resulting match.</param>
        /// <returns>True if it was a match and <paramref name="match"/> has a value; false otherwise.</returns>
        public static bool IsMatch(this Regex regex, string str, out Match match) {
            match = regex.Match(str);
            return match.Success;
        }

        /// <summary>
        ///     Gets the current process's memory usage as a pretty string. Can be shown in Bytes or all the way up to Terabytes via the <paramref name="memType"/> parameter.
        /// </summary>
        /// <param name="process">The current Process.</param>
        /// <param name="memType">The MemoryType to format the string to.</param>
        /// <returns>The formatted string.</returns>
        public static string GetMemoryUsage(this Process process, MemoryType memType = MemoryType.Megabytes)
        {
            return memType switch
            {
                MemoryType.Terabytes =>
                    $"{_divideProcessMemoryTiers(4)} TB",
                MemoryType.Gigabytes =>
                    $"{_divideProcessMemoryTiers(3)} GB",
                MemoryType.Megabytes =>
                    $"{_divideProcessMemoryTiers(2)} MB",
                MemoryType.Kilobytes =>
                    $"{_divideProcessMemoryTiers(1)} KB",
                MemoryType.Bytes =>
                    $"{process.PrivateMemorySize64} B",
                _ => "null"
            };
            
            long _divideProcessMemoryTiers(int divisions)
            {
                var mem = process.PrivateMemorySize64;
                Lambda.Repeat(divisions.CoerceAtMost(4), () =>
                    mem /= _memoryTierSize
                );
                return mem;
            }
        }
        
#nullable enable

        public static Task OrCompleted(this Task? task) => task ?? Task.CompletedTask;


        #region JavaScript Promise-like .Then chaining

        public static async Task Then(this Task task, Func<Task> continuation)
            => await task.ContinueWith(_ => continuation()).ConfigureAwait(false);

        public static async Task<T> ThenApply<T>(this Task<T> task, Func<T, Task> continuation)
        {
            var result = await task;
            await continuation(result);
            return result;
        }
        
        public static async Task<T> ThenApply<T>(this ValueTask<T> task, Func<T, Task> continuation)
        {
            var result = await task;
            await continuation(result);
            return result;
        }
        
        public static async Task ThenUse<T>(this Task<T> task, Func<T, Task> continuation) 
            => await continuation(await task);
        
        
        public static async Task ThenUse<T>(this ValueTask<T> task, Func<T, Task> continuation) 
            => await continuation(await task);
        
        
        public static async Task<TR> Then<T, TR>(this Task<T> task, Func<T, Task<TR>> continuation) => await continuation(await task);
        public static async Task<TR> Then<T, TR>(this ValueTask<T> task, Func<T, Task<TR>> continuation) => await continuation(await task);
        
        public static async Task<TR> Then<T, TR>(this Task<T> task, Func<T, TR> continuation) => continuation(await task);
        public static async Task<TR> Then<T, TR>(this ValueTask<T> task, Func<T, TR> continuation) => continuation(await task);

        #endregion
    }

    public enum MemoryType {
        Terabytes,
        Gigabytes,
        Megabytes,
        Kilobytes,
        Bytes
    }
}