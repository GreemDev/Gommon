using System;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Gommon
{
    /// <summary>
    ///     Utilities that utilize C# lambdas.
    /// </summary>
    public static partial class Lambda
    {
        public static TStruct? TryOrNull<TStruct>(Func<TStruct?> function) where TStruct : struct
        {
            try
            {
                return function();
            }
            catch
            {
                return null;
            }
        }

        public static void Try(Action action) 
        {
            try
            {
                action();
            }
            catch
            {
                // ignored
            }
        }
        
        public static TClass TryOrNull<TClass>(Func<TClass> function) where TClass : class
        {
            try
            {
                return function();
            }
            catch
            {
                return null;
            }
        }
        
        public static async Task<TStruct?> TryOrNullAsync<TStruct>(Func<Task<TStruct?>> function) where TStruct : struct
        {
            try
            {
                return await function();
            }
            catch
            {
                return null;
            }
        }
        
        public static async Task TryAsync(Func<Task> function) 
        {
            try
            {
                await function();
            }
            catch
            {
                // ignored
            }
        }
        
        public static async Task<TClass> TryOrNullAsync<TClass>(Func<Task<TClass>> function) where TClass : class
        {
            try
            {
                return await function();
            }
            catch
            {
                return null;
            }
        }
        

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
        ///     Attempts to run <paramref name="function"/> in a <see langword="try"/> block; returning its value.
        ///     If an error occurs, <paramref name="onError"/> is called, returning its value.
        /// </summary>
        /// <param name="function">The function to run.</param>
        /// <param name="onError">The function called on failure.</param>
        /// <typeparam name="T">The type of the object being returned from either <paramref name="function"/> or <paramref name="onError"/>.</typeparam>
        /// <typeparam name="TException">The specific type of exception to catch. All other exceptions except for one of this type *will* cause a throw.</typeparam>
        /// <returns>The value from <paramref name="function"/>; or <paramref name="onError"/>'s value if <paramref name="function"/> throws an <see cref="Exception"/>.</returns>
        public static T TryCatch<T, TException>(Func<T> function, Func<TException, T> onError)
            where TException : Exception
        {
            T result;
            try
            {
                result = function();
            }
            catch (TException e)
            {
                result = onError(e);
            }
            return result;
        }

        /// <summary>
        ///     Attempts to run <paramref name="function"/> in a <see langword="try"/> block asynchronously; returning its value.
        ///     If an error occurs, <paramref name="onError"/> is called asynchronously, returning its value.
        /// </summary>
        /// <param name="function">The async function to run.</param>
        /// <param name="onError">The async function called on failure.</param>
        /// <typeparam name="T">The type of the object being returned from either <paramref name="function"/> or <paramref name="onError"/>.</typeparam>
        /// <returns>The value from <paramref name="function"/>; or <paramref name="onError"/>'s value if <paramref name="function"/> throws an <see cref="Exception"/>.</returns>
        public static Task<T> TryCatchAsync<T>(Func<Task<T>> function, Func<Exception, Task<T>> onError) 
            => TryCatchAsync<T, Exception>(function, onError);
        

        /// <summary>
        ///     Attempts to run <paramref name="function"/> in a <see langword="try"/> block asynchronously; returning its value.
        ///     If an error occurs, <paramref name="onError"/> is called asynchronously, returning its value.
        /// </summary>
        /// <param name="function">The async function to run.</param>
        /// <param name="onError">The async function called on failure.</param>
        /// <typeparam name="T">The type of the object being returned from either <paramref name="function"/> or <paramref name="onError"/>.</typeparam>
        /// <typeparam name="TException">The specific type of exception to catch. All other exceptions except for one of this type *will* cause a throw.</typeparam>
        /// <returns>The value from <paramref name="function"/>; or <paramref name="onError"/>'s value if <paramref name="function"/> throws an <see cref="Exception"/>.</returns>
        public static async Task<T> TryCatchAsync<T, TException>(Func<Task<T>> function, Func<TException, Task<T>> onError)
            where TException : Exception
        {
            T result;
            try
            {
                result = await function();
            }
            catch (TException e)
            {
                result = await onError(e);
            }
            return result;
        }
        

        /// <summary>
        ///     Executes <paramref name="action"/> the specified amount of <paramref name="times"/>.
        /// </summary>
        /// <param name="times">The amount of times to execute the <paramref name="action"/>.</param>
        /// <param name="action">The <see cref="Action"/> to repeat.</param>
        public static void Repeat([NonNegativeValue] int times, [NotNull] Action action)
        {
            for (var i = 0; i < times; i++)
                action();
        }

        /// <summary>
        ///     Executes <paramref name="function"/> the specified amount of <paramref name="times"/>, awaiting each call.
        /// </summary>
        /// <param name="times">The amount of times to execute the <paramref name="function"/>.</param>
        /// <param name="function">The <see cref="Func{TResult}"/> to repeat asynchronously.</param>
        public static async Task RepeatAsync([NonNegativeValue] int times, [NotNull] Func<Task> function)
        {
            for (var i = 0; i < times; i++)
                await function();
        }

        public static string String(Action<StringBuilder> initializer) =>
            new StringBuilder().Apply(initializer).ToString();
        
        public static async Task<string> StringAsync(Func<StringBuilder, Task> initializer)
        {
            var sb = new StringBuilder();
            await initializer(sb);
            return sb.ToString();
        }
    }
}