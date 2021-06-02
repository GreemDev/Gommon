using System;
using System.Threading.Tasks;

namespace Gommon
{
    /// <summary>
    ///     Utilities that utilize C# lambdas.
    /// </summary>
    public class Lambda
    {

        /// <summary>
        ///     Attempts to run <paramref name="function"/> in a <see langword="try"/> block; returning its value.
        ///     If an error occurs, <paramref name="onError"/> is called, returning its value.
        /// </summary>
        /// <param name="function">The function to run.</param>
        /// <param name="onError">The function called on failure.</param>
        /// <typeparam name="T">The type of the object being returned from either <paramref name="function"/> or <paramref name="onError"/>.</typeparam>
        /// <returns>The value from <paramref name="function"/>; or <paramref name="onError"/>'s value if <paramref name="function"/> throws an <see cref="Exception"/>.</returns>
        public static T TryCatch<T>(Func<T> function, Func<Exception, T> onError)
        {
            T result;
            try
            {
                result = function();
            }
            catch (Exception e)
            {
                result = onError(e);
            }
            return result;
        }

        /// <summary>
        ///     Executes <paramref name="action"/> the specified amount of <paramref name="times"/>.
        /// </summary>
        /// <param name="times">The amount of times to execute the <paramref name="action"/>.</param>
        /// <param name="action">The <see cref="Action"/> to repeat.</param>
        public static void Repeat(int times, Action action)
        {
            for (var i = 0; i < times; i++)
                action();
        }

        /// <summary>
        ///     Executes <paramref name="function"/> the specified amount of <paramref name="times"/>, awaiting each call.
        /// </summary>
        /// <param name="times">The amount of times to execute the <paramref name="function"/>.</param>
        /// <param name="function">The <see cref="Func{TResult}"/> to repeat asynchronously.</param>
        public static async Task RepeatAsync(int times, Func<Task> function)
        {
            for (var i = 0; i < times; i++)
                await function();
        }
    }
}