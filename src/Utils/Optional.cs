using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

/*/
 * This is a 1:1 copy, with name & syntax changes, of .NEXT's 2.x Optional file.
 * I am not claiming credit for this, I just don't want to use a massive, rich API for a single concept it offers.
/*/

// ReSharper disable MemberCanBePrivate.Global
namespace Gommon {
    /// <summary>
    /// Various extension and factory methods for constructing optional value.
    /// </summary>
    public static class Optional {
        /// <summary>
        ///     Wraps <i>any</i> value, null or otherwise (with no <see cref="NullReferenceException"/>s), in an <see cref="Optional{T}"/>.
        /// </summary>
        /// <param name="obj">The current object.</param>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <returns>An optional, containing the possibly-null value of <paramref name="obj"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Optional<T> AsOptional<T>(this T obj) => new(obj);
        
        /// <summary>
        ///     Wraps the result of <paramref name="task"/> which can be <i>any</i> value, null or otherwise (with no <see cref="NullReferenceException"/>s), in an <see cref="Optional{T}"/>.
        /// </summary>
        /// <param name="task">The current task.</param>
        /// <typeparam name="T">The type of the task's result.</typeparam>
        /// <returns>An optional, containing the possibly-null result of <paramref name="task"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<Optional<T>> AsOptional<T>(this Task<T> task) => new(await task);

        /// <summary>
        /// If a value is present, returns the value, otherwise <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="task">The task returning optional value.</param>
        /// <returns>Nullable value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<T?> OrNull<T>(this Task<Optional<T>> task)
            where T : struct
            => (await task.ConfigureAwait(false)).OrNull();

        /// <summary>
        /// Returns the value if present; otherwise return default value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="task">The task returning optional value.</param>
        /// <param name="defaultValue">The value to be returned if there is no value present.</param>
        /// <returns>The value, if present, otherwise default.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<T> OrElse<T>(this Task<Optional<T>> task, T defaultValue)
            => (await task.ConfigureAwait(false)).OrElse(defaultValue);

        /// <summary>
        /// If a value is present, returns the value, otherwise throw exception.
        /// </summary>
        /// <param name="task">The task returning optional value.</param>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <typeparam name="TException">Type of exception to throw.</typeparam>
        /// <returns>The value, if present.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<T> OrThrow<T, TException>(this Task<Optional<T>> task)
            where TException : Exception, new()
            => (await task.ConfigureAwait(false)).OrThrow<TException>();

        /// <summary>
        /// If a value is present, returns the value, otherwise throw exception.
        /// </summary>
        /// <returns>The value, if present.</returns>
        [return: NotNull]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<T> OrThrow<T>(this Task<Optional<T>> task)
            => (await task.ConfigureAwait(false)).OrThrow();

        /// <summary>
        /// If a value is present, returns the value, otherwise throw exception.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <typeparam name="TException">Type of exception to throw.</typeparam>
        /// <param name="task">The task returning optional value.</param>
        /// <param name="exceptionFactory">Exception factory.</param>
        /// <returns>The value, if present.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<T> OrThrow<T, TException>(this Task<Optional<T>> task,
            Func<TException> exceptionFactory)
            where TException : Exception
            => (await task.ConfigureAwait(false)).OrThrow(exceptionFactory);

        /// <summary>
        /// If a value is present, returns the value, otherwise throw exception with specified <paramref name="message"/>.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="task">The task returning optional value.</param>
        /// <param name="message">The message of the exception.</param>
        /// <returns>The value, if present.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<T> OrThrow<T>(this Task<Optional<T>> task, string message)
            => (await task.ConfigureAwait(false)).OrThrow(() => new ValueException(message));

        /// <summary>
        /// If a value is present, returns the value, otherwise throw exception.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <typeparam name="TException">Type of exception to throw.</typeparam>
        /// <param name="task">The task returning optional value.</param>
        /// <param name="exceptionFactory">Exception factory.</param>
        /// <returns>The value, if present.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<T> OrThrow<T, TException>(this Task<Optional<T>> task,
            Func<Task<TException>> exceptionFactory)
            where TException : Exception
            => await (await task.ConfigureAwait(false)).OrThrow(exceptionFactory).ConfigureAwait(false);

        /// <summary>
        /// Returns the value if present; otherwise invoke delegate.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="task">The task returning optional value.</param>
        /// <param name="defaultFunc">A delegate to be invoked if value is not present.</param>
        /// <returns>The value, if present, otherwise returned from delegate.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<T> OrElseGet<T>(this Task<Optional<T>> task, Func<T> defaultFunc)
            => (await task.ConfigureAwait(false)).OrElseGet(defaultFunc);

        /// <summary>
        /// Returns the value if present; otherwise invoke delegate.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="task">The task returning optional value.</param>
        /// <param name="defaultFunc">A delegate to be invoked if value is not present.</param>
        /// <returns>The value, if present, otherwise returned from delegate.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<T> OrElseGet<T>(this Task<Optional<T>> task, Func<Task<T>> defaultFunc)
            => await (await task.ConfigureAwait(false)).OrElseGet(defaultFunc).ConfigureAwait(false);

        /// <summary>
        ///     Checks whether or not the value described in this Optional is present and matches the given <paramref name="condition"/> delegate.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="task">The task returning optional value.</param>
        /// <param name="condition">The predicate to check.</param>
        /// <returns><see langword="true"/>, if this Optional has a value and matches <paramref name="condition"/>; <see langword="false"/> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<bool> Check<T>(this Task<Optional<T>> task, Func<T, bool> condition)
            => (await task.ConfigureAwait(false)).Check(condition);

        /// <summary>
        ///     Checks whether or not the value described in this Optional is present and matches the given <paramref name="condition"/> delegate.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="task">The task returning optional value.</param>
        /// <param name="condition">The predicate to check.</param>
        /// <returns><see langword="true"/>, if this Optional has a value and matches <paramref name="condition"/>; <see langword="false"/> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<bool> Check<T>(this Task<Optional<T>> task, Func<T, Task<bool>> condition)
            => await (await task.ConfigureAwait(false)).Check(condition).ConfigureAwait(false);

        /// <summary>
        /// If a value is present, returns the value, otherwise return default value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="task">The task returning optional value.</param>
        /// <returns>The value, if present, otherwise default.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<T> OrDefault<T>(this Task<Optional<T>> task)
            => (await task.ConfigureAwait(false)).OrDefault();

        /// <summary>
        /// If a value is present, and the value matches the given predicate,
        /// return an Optional describing the value, otherwise return an empty Optional.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="task">The task returning optional value.</param>
        /// <param name="condition">A predicate to apply to the value, if present.</param>
        /// <returns>An Optional describing the value of this Optional if a value is present and the value matches the given predicate, otherwise an empty Optional.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<Optional<T>> OnlyIf<T>(this Task<Optional<T>> task, Predicate<T> condition)
            => (await task.ConfigureAwait(false)).OnlyIf(condition);

        /// <summary>
        /// If a value is present, and the value matches the given predicate,
        /// return an Optional describing the value, otherwise return an empty Optional.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="task">The task returning optional value.</param>
        /// <param name="condition">A predicate to apply to the value, if present.</param>
        /// <returns>An Optional describing the value of this Optional if a value is present and the value matches the given predicate, otherwise an empty Optional.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<Optional<T>> OnlyIf<T>(this Task<Optional<T>> task, Func<T, Task<bool>> condition)
            => await (await task.ConfigureAwait(false)).OnlyIf(condition).ConfigureAwait(false);

        /// <summary>
        /// Returns the current optional, and of the value is present, invoke an action.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="task">The task returning optional value.</param>
        /// <param name="action">An action to invoke on the value, if present.</param>
        /// <returns>The current optional.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<Optional<T>> IfPresent<T>(this Task<Optional<T>> task, Action<T> action)
            => (await task.ConfigureAwait(false)).IfPresent(action);

        /// <summary>
        /// Returns the current optional, and of the value is present, invoke an action.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="task">The task returning optional value.</param>
        /// <param name="action">An action to invoke on the value, if present.</param>
        /// <returns>The current optional.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task<Optional<T>> IfPresent<T>(this Task<Optional<T>> task, Func<T, Task> action)
            => await (await task.ConfigureAwait(false)).IfPresent(action).ConfigureAwait(false);

        /// <summary>
        /// Indicates that specified type is optional type.
        /// </summary>
        /// <param name="optionalType">The type to check.</param>
        /// <returns><see langword="true"/>, if specified type is optional type; otherwise, <see langword="false"/>.</returns>
        public static bool IsOptional(this Type optionalType) => optionalType.IsConstructedGenericType &&
                                                                 optionalType.GetGenericTypeDefinition()
                                                                 == typeof(Optional<>);

        /// <summary>
        /// Returns the underlying type argument of the specified optional type.
        /// </summary>
        /// <param name="optionalType">Optional type.</param>
        /// <returns>Underlying type argument of optional type; otherwise, <see langword="null"/>.</returns>
        public static Type GetUnderlyingType(Type optionalType) =>
            IsOptional(optionalType) ? optionalType.GetGenericArguments()[0] : null;

        /// <summary>
        /// Constructs optional value from nullable reference type.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>The value wrapped into Optional container.</returns>
        public static Optional<T> AsOptional<T>(this in T? value)
            where T : struct
            => value ?? None<T>();

        /// <summary>
        /// If a value is present, returns the value, otherwise <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="value">Optional value.</param>
        /// <returns>Nullable value.</returns>
        public static T? OrNull<T>(this in Optional<T> value) where T : struct
            => value.TryGet(out var result) ? new T?(result) : null;

        /// <summary>
        /// Returns the second value if the first is empty.
        /// </summary>
        /// <param name="first">The first optional value.</param>
        /// <param name="second">The second optional value.</param>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <returns>The second value if the first is empty; otherwise, the first value.</returns>
        public static ref readonly Optional<T> Coalesce<T>(this in Optional<T> first, in Optional<T> second) =>
            ref first.HasValue ? ref first : ref second;

        /// <summary>
        /// Returns empty value.
        /// </summary>
        /// <typeparam name="T">The type of empty result.</typeparam>
        /// <returns>The empty value.</returns>
        public static Optional<T> None<T>() => Optional<T>.None;
#nullable enable
        /// <summary>
        /// Wraps the value to <see cref="Optional{T}"/> container.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>The optional container.</returns>
        public static Optional<T> Of<T>(T? value) => new(value);
        
        /// <summary>
        /// Wraps <see langword="null"/> value to <see cref="Optional{T}"/> container.
        /// </summary>
        /// <typeparam name="T">The reference type.</typeparam>
        /// <returns>The <see cref="Optional{T}"/> instance representing <see langword="null"/> value.</returns>
        public static Optional<T?> Null<T>() where T : class
            => Of<T?>(null);
#nullable disable
    }

    /// <summary>
    /// A container object which may or may not contain a value.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    [StructLayout(LayoutKind.Auto)]
    public readonly struct Optional<T> : IEquatable<Optional<T>>, IEquatable<T>, IDisposable {
        private const byte _undefinedValue = 0;
        private const byte _nullValue = 1;
        private const byte _notEmptyValue = 3;

        // ReSharper disable once StaticMemberInGenericType
        private static readonly bool IsOptional;

        static Optional() {
            var type = typeof(T);
            IsOptional = type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Optional<>);
        }

        [AllowNull] private readonly T _value;
        private readonly byte _valueKind;

        /// <summary>
        /// Constructs non-empty container.
        /// </summary>
        /// <param name="value">A value to be placed into container.</param>
        /// <remarks>
        /// The property <see langword="IsNull"/> of the constructed object may be <see langword="true"/>
        /// if <paramref name="value"/> is <see langword="null"/>.
        /// The property <see langword="IsUndefined"/> of the constructed object is always <see langword="false"/>.
        /// </remarks>
#pragma warning disable CS8618
        public Optional([AllowNull] T value)
#pragma warning restore CS8618
        {
            _value = value;
            if (value is null)
                _valueKind = _nullValue;
            else if (IsOptional)
                _valueKind = GetKindUnsafe(ref value!);
            else
                _valueKind = _notEmptyValue;
        }

#pragma warning disable CS8618
        public Optional(Func<T> getValue) : this(getValue()) { }
#pragma warning restore CS8618

        private static byte GetKindUnsafe([DisallowNull] ref T optionalValue) {
            if (optionalValue.Equals(null))
                return _nullValue;

            return optionalValue.Equals(Missing.Value) 
                ? _undefinedValue 
                : _notEmptyValue;
        }

        /// <summary>
        /// Represents optional container without value.
        /// </summary>
        /// <remarks>
        /// The property <see cref="IsUndefined"/> of returned object is always <see langword="true"/>.
        /// </remarks>
        public static Optional<T> None => default;

        /// <summary>
        /// Indicates whether the value is present.
        /// </summary>
        /// <remarks>
        /// If this property is <see langword="true"/> then <see cref="IsUndefined"/> and <see cref="IsNull"/>
        /// equal to <see langword="false"/>.
        /// </remarks>
        public bool HasValue => _valueKind == _notEmptyValue;

        /// <summary>
        /// Indicates that the value is undefined.
        /// </summary>
        /// <seealso cref="None"/>
        public bool IsUndefined => _valueKind == _undefinedValue;

        /// <summary>
        /// Indicates that the value is <see langword="null"/>.
        /// </summary>
        /// <remarks>
        /// This property returns <see langword="true"/> only if this instance
        /// was constructed using <see cref="Optional{T}(T)"/> with <see langword="null"/> argument.
        /// </remarks>
        public bool IsNull => _valueKind == _nullValue;

        /// <summary>
        /// Boxes value encapsulated by this object.
        /// </summary>
        /// <returns>The boxed value.</returns>
        public Optional<object> Box() => HasValue ? new Optional<object>(_value!) : default;

        /// <summary>
        /// Attempts to extract value from container if it is present.
        /// </summary>
        /// <param name="value">Extracted value.</param>
        /// <returns><see langword="true"/> if value is present; otherwise, <see langword="false"/>.</returns>
        public bool TryGet([NotNullWhen(true)] out T value) {
            value = _value;
            return HasValue;
        }

        /// <summary>
        /// Returns the value if present; otherwise return default value.
        /// </summary>
        /// <param name="defaultValue">The value to be returned if there is no value present.</param>
        /// <returns>The value, if present, otherwise <paramref name="defaultValue"/>.</returns>
        [return: NotNullIfNotNull("defaultValue")]
        public T OrElse(T defaultValue) => HasValue ? _value : defaultValue;

        /// <summary>
        /// If a value is present, returns the value, otherwise throw exception.
        /// </summary>
        /// <typeparam name="TException">Type of exception to throw.</typeparam>
        /// <returns>The value, if present.</returns>
        [return: NotNull]
        public T OrThrow<TException>()
            where TException : Exception, new()
            => HasValue ? _value! : throw new TException();

        /// <summary>
        /// If a value is present, returns the value, otherwise throw exception with default message.
        /// </summary>
        /// <returns>The value, if present.</returns>
        [return: NotNull]
        public T OrThrow() => OrThrow($"Value of type '{typeof(T).AsPrettyString()}' is absent.");
        
        /// <summary>
        /// If a value is present, returns the value, otherwise throw exception with the specified <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message of the exception to throw.</param>
        /// <returns>The value, if present.</returns>
        [return: NotNull]
        public T OrThrow(string message) =>
            OrThrow(() => new ValueException(message));

        /// <summary>
        /// If a value is present, returns the value, otherwise throw exception.
        /// </summary>
        /// <typeparam name="TException">Type of exception to throw.</typeparam>
        /// <param name="exceptionFactory">Exception factory.</param>
        /// <returns>The value, if present.</returns>
        [return: NotNull]
        public T OrThrow<TException>(Func<TException> exceptionFactory)
            where TException : Exception
            => HasValue ? _value! : throw exceptionFactory();

        /// <summary>
        /// If a value is present, returns the value, otherwise throw exception.
        /// </summary>
        /// <typeparam name="TException">Type of exception to throw.</typeparam>
        /// <param name="exceptionFactory">Exception factory.</param>
        /// <returns>The value, if present.</returns>
        [return: NotNull]
        public async Task<T> OrThrow<TException>(Func<Task<TException>> exceptionFactory)
            where TException : Exception
            => HasValue ? _value! : throw await exceptionFactory();

        /// <summary>
        /// Returns the value if present; otherwise invoke delegate.
        /// </summary>
        /// <param name="defaultFunc">A delegate to be invoked if value is not present.</param>
        /// <returns>The value, if present, otherwise returned from delegate.</returns>
        public T OrElseGet(in Func<T> defaultFunc) => HasValue ? _value : defaultFunc();

        /// <summary>
        /// Returns the value if present; otherwise invoke delegate.
        /// </summary>
        /// <param name="defaultFunc">A delegate to be invoked if value is not present.</param>
        /// <returns>The value, if present, otherwise returned from delegate.</returns>
        public async Task<T> OrElseGet(Func<Task<T>> defaultFunc) => HasValue ? _value : await defaultFunc();

        /// <summary>
        ///     Checks whether the value described in this Optional is present and matches the given <paramref name="condition"/> delegate.
        /// </summary>
        /// <param name="condition">The predicate to check.</param>
        /// <returns><see langword="true"/>, if this Optional has a value and matches <paramref name="condition"/>; <see langword="false"/> otherwise.</returns>
        public bool Check(in Func<T, bool> condition) => HasValue && condition(_value);

        /// <summary>
        ///     Checks whether the value described in this Optional is present and matches the given <paramref name="condition"/> delegate.
        /// </summary>
        /// <param name="condition">The predicate to check.</param>
        /// <returns><see langword="true"/>, if this Optional has a value and matches <paramref name="condition"/>; <see langword="false"/> otherwise.</returns>
        public async Task<bool> Check(Func<T, Task<bool>> condition) => HasValue && await condition(_value);

        /// <summary>
        /// If a value is present, returns the value, otherwise return default value.
        /// </summary>
        /// <returns>The value, if present, otherwise default.</returns>
        public T OrDefault() => _value;

        /// <summary>
        /// If a value is present, returns the value, otherwise throw exception.
        /// </summary>
        /// <exception cref="InvalidOperationException">No value is present.</exception>
        [DisallowNull]
        public T Value {
            get {
                string msg;
                switch (_valueKind) {
                    default:
                        return _value;
                    case _undefinedValue:
                        msg = "The underlying value is undefined";
                        break;
                    case _nullValue:
                        msg = "The underlying value is null";
                        break;
                }

                throw new InvalidOperationException(msg);
            }
        }

        /// <summary>
        /// If a value is present, apply the provided mapping function to it, and if the result is
        /// non-null, return an Optional describing the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the mapping function.</typeparam>
        /// <param name="mapper">A mapping function to be applied to the value, if present.</param>
        /// <returns>An Optional describing the result of applying a mapping function to the value of this Optional, if a value is present, otherwise <see cref="None"/>.</returns>
        public Optional<TResult> Convert<TResult>(in Func<T, TResult> mapper)
            => HasValue ? mapper(_value) : Optional<TResult>.None;

        /// <summary>
        /// If a value is present, apply the provided mapping function to it, and if the result is
        /// non-null, return an Optional describing the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the mapping function.</typeparam>
        /// <param name="mapper">A mapping function to be applied to the value, if present.</param>
        /// <returns>An Optional describing the result of applying a mapping function to the value of this Optional, if a value is present, otherwise <see cref="None"/>.</returns>
        public Optional<TResult> Convert<TResult>(in Func<T, Optional<TResult>> mapper)
            => HasValue ? mapper(_value) : Optional<TResult>.None;

        /// <summary>
        /// If a value is present, apply the provided mapping function to it, and if the result is
        /// non-null, return an Optional describing the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the mapping function.</typeparam>
        /// <param name="mapper">A mapping function to be applied to the value, if present.</param>
        /// <returns>An Optional describing the result of applying a mapping function to the value of this Optional, if a value is present, otherwise <see cref="None"/>.</returns>
        public async Task<Optional<TResult>> Convert<TResult>(Func<T, Task<TResult>> mapper)
            => HasValue ? await mapper(_value) : Optional<TResult>.None;

        /// <summary>
        /// If a value is present, apply the provided mapping function to it, and if the result is
        /// non-null, return an Optional describing the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the mapping function.</typeparam>
        /// <param name="mapper">A mapping function to be applied to the value, if present.</param>
        /// <returns>An Optional describing the result of applying a mapping function to the value of this Optional, if a value is present, otherwise <see cref="None"/>.</returns>
        public async Task<Optional<TResult>> Convert<TResult>(Func<T, Task<Optional<TResult>>> mapper)
            => HasValue ? await mapper(_value) : Optional<TResult>.None;

        /// <summary>
        /// If a value is present, and the value matches the given predicate,
        /// return an Optional describing the value, otherwise return an empty Optional.
        /// </summary>
        /// <param name="condition">A predicate to apply to the value, if present.</param>
        /// <returns>An Optional describing the value of this Optional if a value is present and the value matches the given predicate, otherwise an empty Optional.</returns>
        public Optional<T> OnlyIf(in Func<T, bool> condition)
            => HasValue && condition(_value) ? this : None;

        /// <summary>
        /// If a value is present, and the value matches the given predicate,
        /// return an Optional describing the value, otherwise return an empty Optional.
        /// </summary>
        /// <param name="condition">A predicate to apply to the value, if present.</param>
        /// <returns>An Optional describing the value of this Optional if a value is present and the value matches the given predicate, otherwise an empty Optional.</returns>
        public Optional<T> OnlyIf(Predicate<T> condition) => HasValue && condition(_value) ? this : None;

        /// <summary>
        /// If a value is present, and the value matches the given predicate,
        /// return an Optional describing the value, otherwise return an empty Optional.
        /// </summary>
        /// <param name="condition">A predicate to apply to the value, if present.</param>
        /// <returns>An Optional describing the value of this Optional if a value is present and the value matches the given predicate, otherwise an empty Optional.</returns>
        public async Task<Optional<T>> OnlyIf(Func<T, Task<bool>> condition)
            => HasValue && await condition(_value) ? this : None;

        /// <summary>
        /// Returns the current optional, and of the value is present, invoke an action.
        /// </summary>
        /// <param name="action">An action to invoke on the value, if present.</param>
        /// <returns>The current optional.</returns>
        public Optional<T> IfPresent(in Action<T> action) {
            if (HasValue)
                action(_value);
            return this;
        }

        /// <summary>
        /// Returns the current optional, and of the value is present, invoke an action.
        /// </summary>
        /// <param name="action">An action to invoke on the value, if present.</param>
        /// <returns>The current optional.</returns>
        public async Task<Optional<T>> IfPresent(Func<T, Task> action) {
            if (HasValue)
                await action(_value);
            return this;
        }

        /// <summary>
        /// Returns textual representation of this object.
        /// </summary>
        /// <returns>The textual representation of this object.</returns>
        public override string ToString() => _valueKind switch {
            _undefinedValue => "<Undefined>",
            _nullValue => "<Null>",
            _ => _value!.ToString()
        };

        /// <summary>
        /// Computes hash code of the stored value.
        /// </summary>
        /// <returns>The hash code of the stored value.</returns>
        /// <remarks>
        /// This method calls <see cref="object.GetHashCode()"/>
        /// for the object <see cref="Value"/>.
        /// </remarks>
        public override int GetHashCode() => HasValue ? EqualityComparer<T>.Default.GetHashCode(_value) : 0;

        /// <summary>
        /// Determines whether this container stored the same
        /// value as the specified value.
        /// </summary>
        /// <param name="other">Other value to compare.</param>
        /// <returns><see langword="true"/> if <see cref="Value"/> is equal to <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        public bool Equals(T other) => HasValue && EqualityComparer<T>.Default.Equals(_value, other);

        private bool Equals(in Optional<T> other) 
            => (_valueKind + other._valueKind) switch
            {
                _notEmptyValue or _notEmptyValue + _nullValue => false,
                _notEmptyValue + _notEmptyValue => EqualityComparer<T>.Default.Equals(_value, other._value),
                _ => true
            };

        /// <summary>
        /// Determines whether this container stores
        /// the same value as other.
        /// </summary>
        /// <param name="other">Other container to compare.</param>
        /// <returns><see langword="true"/> if this container stores the same value as <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Optional<T> other) => Equals(in other);

        /// <summary>
        /// Determines whether this container stores the same value as <paramref name="other"/>.
        /// </summary>
        /// <param name="other">Other container to compare.</param>
        /// <returns><see langword="true"/> if this container stores the same value as <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
#nullable enable
        public override bool Equals(object? other) {
#nullable disable
            switch (other) {
                case null:
                    return _valueKind == _nullValue;
                case Optional<T> optional:
                    return Equals(in optional);
                case T value:
                    return Equals(value);
            }

            if (ReferenceEquals(other, Missing.Value))
                return _valueKind == _undefinedValue;

            return false;
        }

        /// <summary>
        /// Wraps value into Optional container.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Optional<T>([AllowNull] T value) =>
            value is null ? Optional.None<T>() : new Optional<T>(value);

        /// <summary>
        /// Extracts value stored in the Optional container.
        /// </summary>
        /// <param name="optional">The container.</param>
        /// <exception cref="InvalidOperationException">No value is present.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        #nullable enable
        public static implicit operator T?(Optional<T> optional) => optional.OrDefault();
        #nullable disable

        /// <summary>
        /// Determines whether two containers store the same value.
        /// </summary>
        /// <param name="first">The first container to compare.</param>
        /// <param name="second">The second container to compare.</param>
        /// <returns><see langword="true"/>, if both containers store the same value; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(in Optional<T> first, in Optional<T> second)
            => first.Equals(in second);

        /// <summary>
        /// Determines whether two containers store the different values.
        /// </summary>
        /// <param name="first">The first container to compare.</param>
        /// <param name="second">The second container to compare.</param>
        /// <returns><see langword="true"/>, if both containers store the different values; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(in Optional<T> first, in Optional<T> second)
            => !first.Equals(in second);

        /// <summary>
        /// Returns non-empty container.
        /// </summary>
        /// <param name="first">The first container.</param>
        /// <param name="second">The second container.</param>
        /// <returns>The first non-empty container.</returns>
        /// <seealso cref="Optional.Coalesce{T}"/>
        public static Optional<T> operator |(in Optional<T> first, in Optional<T> second)
            => first.Coalesce(in second);

        /// <summary>
        /// Determines whether two containers are empty or have values.
        /// </summary>
        /// <param name="first">The first container.</param>
        /// <param name="second">The second container.</param>
        /// <returns><see langword="true"/>, if both containers are empty or have values; otherwise, <see langword="false"/>.</returns>
        public static Optional<T> operator ^(in Optional<T> first, in Optional<T> second)
        {
            return (first._valueKind - second._valueKind) switch
            {
                _undefinedValue - _nullValue or 
                    _nullValue - _notEmptyValue or 
                    _undefinedValue - _notEmptyValue 
                    => second,
                _notEmptyValue - _undefinedValue or 
                    _notEmptyValue - _nullValue or 
                    _nullValue - _undefinedValue 
                    => first,
                _ => default
            };
        }

        /// <summary>
        /// Checks whether the container has value.
        /// </summary>
        /// <param name="optional">The container to check.</param>
        /// <returns><see langword="true"/> if this container has value; otherwise, <see langword="false"/>.</returns>
        /// <see cref="HasValue"/>
        public static bool operator true(Optional<T> optional) => optional.HasValue;

        /// <summary>
        /// Checks whether the container has no value.
        /// </summary>
        /// <param name="optional">The container to check.</param>
        /// <returns><see langword="true"/> if this container has no value; otherwise, <see langword="false"/>.</returns>
        /// <see cref="HasValue"/>
        public static bool operator false(Optional<T> optional) => optional._valueKind < _notEmptyValue;

        /// <inheritdoc/>
        public void Dispose() => _value.Cast<IDisposable>()?.Dispose();
    }
}