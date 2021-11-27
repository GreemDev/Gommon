using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gommon
{
    public static class Executor
    {
        /// <summary>
        ///     Executes the <paramref name="func"/> asynchronously after the specified <paramref name="delay"/>.
        /// </summary>
        /// <param name="delay">Delay, in TimeSpan.</param>
        /// <param name="func">Function, typically async.</param>
        /// <returns><see cref="Task"/></returns>
        public static async Task ExecuteAfterDelayAsync(TimeSpan delay, Func<Task> func)
            => await Task.Delay(delay).ContinueWith(async _ => await func().ConfigureAwait(false)).ConfigureAwait(false);

        /// <summary>
        ///     Executes the <paramref name="func"/> asynchronously.
        /// </summary>
        /// <param name="func">Function, typically async.</param>
        /// <returns><see cref="Task"/></returns>
        public static async Task ExecuteAsync(Func<Task> func)
            => await Task.Run(async () => await func().ConfigureAwait(false)).ConfigureAwait(false);

        /// <summary>
        ///     Executes the <paramref name="func"/> in the background, via an unawaited Task.Run.
        /// </summary>
        /// <param name="func">Function, typically async.</param>
        public static void ExecuteBackgroundAsync(Func<Task> func) => _ = Task.Run(func);

        /// <summary>
        ///     Executes the <paramref name="action"/> in the background, via an unawaited Task.Run.
        /// </summary>
        /// <param name="action">Synchronous function to execute. Avoid using the <code>async</code> modifier, as that creates an <code>async void</code>.</param>
        public static void ExecuteBackground(Action action) => _ = Task.Run(action);

        /// <summary>
        ///     Executes the <paramref name="action"/> synchronously in a separate thread after the specified <paramref name="delay"/> delay.
        /// </summary>
        /// <param name="delay">Delay, in TimeSpan.</param>
        /// <param name="action">Synchronous function to execute. Avoid using the <code>async</code> modifier, as that creates an <code>async void</code>.</param>
        public static void ExecuteAfterDelay(TimeSpan delay, Action action)
            => new Thread(() =>
            {
                try
                {
                    Thread.Sleep(delay);
                    action();
                }
                catch
                {
                    //ignored
                }
            }).Start();

        /// <summary>
        ///     Executes the <paramref name="action"/> synchronously in a separate thread.
        /// </summary>
        /// <param name="action">Synchronous function to execute. Avoid using the <code>async</code> modifier, as that creates an <code>async void</code> which can crash your program.</param>
        public static void Execute(Action action)
            => new Thread(() =>
            {
                try
                {
                    action(); 
                }
                catch
                {
                    //ignored
                }
            }).Start();
    }
}
