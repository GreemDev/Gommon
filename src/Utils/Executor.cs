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
            => await Task.Delay(delay).ContinueWith(async _ => await func());

        /// <summary>
        ///     Executes the <paramref name="func"/> asynchronously after the specified <paramref name="ms"/> delay.
        /// </summary>
        /// <param name="ms">Delay, in milliseconds.</param>
        /// <param name="func">Function, typically async.</param>
        /// <returns><see cref="Task"/></returns>
        public static async Task ExecuteAfterDelayAsync(int ms, Func<Task> func)
            => await Task.Delay(ms).ContinueWith(async _ => await func());

        /// <summary>
        ///     Executes the <paramref name="func"/> asynchronously.
        /// </summary>
        /// <param name="func">Function, typically async.</param>
        /// <returns><see cref="Task"/></returns>
        public static async Task ExecuteAsync(Func<Task> func)
            => await Task.Run(async () => await func());

        /// <summary>
        ///     Executes the <paramref name="action"/> synchronously in a separate thread after the specified <paramref name="delay"/> delay.
        /// </summary>
        /// <param name="delay">Delay, in TimeSpan.</param>
        /// <param name="action">Synchronous function to execute. Avoid using the <code>async</code> modifier.</param>
        public static void ExecuteAfterDelay(TimeSpan delay, Action action)
            => new Thread(() =>
            {
                Thread.Sleep(delay);
                action();
            }).Start();

        /// <summary>
        ///     Executes the <paramref name="action"/> synchronously in a separate thread after the specified <paramref name="ms"/> delay.
        /// </summary>
        /// <param name="ms">Delay, in milliseconds.</param>
        /// <param name="action">Synchronous function to execute. Avoid using the <code>async</code> modifier, as that creates an <code>async void</code>.</param>
        public static void ExecuteAfterDelay(int ms, Action action)
            => new Thread(() =>
            {
                Thread.Sleep(ms);
                action();
            }).Start();

        /// <summary>
        ///     Executes the <paramref name="action"/> synchronously in a separate thread.
        /// </summary>
        /// <param name="action">Synchronous function to execute. Avoid using the <code>async</code> modifier.</param>
        public static void Execute(Action action)
            => new Thread(action.Invoke).Start();
    }
}
