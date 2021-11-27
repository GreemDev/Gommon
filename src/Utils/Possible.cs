using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Gommon {
    /// <summary>
    ///     A struct, containing a possible value with methods providing a fluent interface for dealing with nullable values.
    /// </summary>
    /// <typeparam name="TValue">The type of value. May only be a class. For structs, use <see cref="PossibleRef{TValue}"/>.</typeparam>
    public struct Possible<TValue> : ICloneable, IDisposable {
        private TValue _value;
        
        /// <summary>
        ///     One of two constructors of this class, accepting a nullable value.
        /// </summary>
        /// <param name="value">The nullable value of this <see cref="Possible{TValue}"/>.</param>
        public Possible(object value = null) : this(() => value) { }

        /// <summary>
        ///     One of two constructors of this class, accepting a function that produces a nullable generic object.
        ///     If the function produces any exceptions, the <see cref="Possible{TValue}"/> is empty.
        /// </summary>
        /// <param name="initializer">The nullable value of this <see cref="Possible{TValue}"/>.</param>
        public Possible([NotNull] Func<object> initializer) {
            try {
                var val = initializer();
                _value = (typeof(TValue) == typeof(string)
                    ? val?.ToString()
                    : val).Cast<TValue>();
            }
            catch {
                _value = default;
            }
        }

        /// <summary>
        ///     Returns the current optional, or an empty one if the result of <paramref name="predicate"/> is <c>false</c>.
        /// </summary>
        /// <param name="predicate">The predicate function to test the value.</param>
        /// <returns>An optional, containing no value if <paramref name="predicate"/> returns <c>false</c>.</returns>
        public Possible<TValue> OnlyIf([NotNull] Func<TValue, bool> predicate)
            => _value != null && predicate(OrNull()) ? this : new Possible<TValue>();

        /// <summary>
        ///     Coerces the current <see cref="Possible{TValue}"/>'s value into a value of <typeparamref name="T"/>. 
        /// </summary>
        /// <param name="transformation">The coercion function.</param>
        /// <typeparam name="T">The type of the new <see cref="Possible{TValue}"/>.</typeparam>
        /// <returns>The new <see cref="Possible{TValue}"/>.</returns>
        public Possible<T> Transform<T>([NotNull] Func<TValue, T> transformation)
            where T : class
            => _value != null
                ? new Possible<T>(transformation(OrNull()))
                : new Possible<T>();

        /// <summary>
        ///     Coerces the current <see cref="Possible{TValue}"/>'s value into a value of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="transformation">The coercion function.</param>
        /// <typeparam name="T">The type of the new <see cref="Possible{TValue}"/>.</typeparam>
        /// <returns>The new <see cref="Possible{TValue}"/>.</returns>
        public Possible<T> Transform<T>([NotNull] Func<TValue, Possible<T>> transformation)
            where T : class
            => _value != null
                ? transformation(OrNull())
                : new Possible<T>();

        /// <summary>
        ///     If this <see cref="Possible{TValue}"/>'s value is not null, <paramref name="action"/> is run.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>The current <see cref="Possible{TValue}"/>.</returns>
        public Possible<TValue> IfPresent([NotNull] Action<TValue> action) {
            if (OrNull() != null)
                action(OrNull());
            return this;
        }

        /// <summary>
        ///     Whether or not the value of this <see cref="Possible{TValue}"/> is null.
        /// </summary>
        public bool IsEmpty => _value is null;

        /// <summary>
        ///     Returns the value of this <see cref="Possible{TValue}"/>.
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown when the value of this <see cref="Possible{TValue}"/> is null.</exception>
        /// <returns>The value of this <see cref="Possible{TValue}"/>.</returns>
        public TValue OrThrow()
            => OrThrow(new NullReferenceException($"value of {GetType().AsPrettyString()} is null."));

        /// <summary>
        ///     Returns the value of this <see cref="Possible{TValue}"/>.
        ///     If the value is null, the value of <paramref name="exception"/> is thrown.
        /// </summary>
        /// <param name="exception">The exception to throw when this <see cref="Possible{TValue}"/>'s value is null.</param>
        /// <returns>The value of this <see cref="Possible{TValue}"/>.</returns>
        public TValue OrThrow<TException>([NotNull] TException exception) where TException : Exception =>
            _value ?? throw exception;

        /// <summary>
        ///     Returns the value of this <see cref="Possible{TValue}"/>.
        ///     If the value is null, the return value of the function <paramref name="exceptionProducer"/> is thrown.
        /// </summary>
        /// <param name="exceptionProducer">The function that produces the exception to throw when this <see cref="Possible{TValue}"/>'s value is null.</param>
        /// <returns>The value of this <see cref="Possible{TValue}"/>.</returns>
        public TValue OrThrow([NotNull] Func<Exception> exceptionProducer) => OrThrow(exceptionProducer());

        /// <summary>
        ///     Returns the value of this <see cref="Possible{TValue}"/>.
        ///     If the value is null, the value of <paramref name="valueProducer"/> is returned instead.
        /// </summary>
        /// <param name="valueProducer">The value to return when this <see cref="Possible{TValue}"/>'s value is null.</param>
        /// <returns>The value of this <see cref="Possible{TValue}"/>, or the result of <paramref name="valueProducer"/>.</returns>
        public TValue OrElseGet([NotNull] Func<TValue> valueProducer) => _value ?? valueProducer();
        
        /// <summary>
        ///     Returns the value of this <see cref="Possible{TValue}"/>.
        ///     If the value is null, the value of <paramref name="value"/> is returned instead.
        /// </summary>
        /// <param name="value">The value to return when this <see cref="Possible{TValue}"/>'s value is null.</param>
        /// <returns>The value of this <see cref="Possible{TValue}"/>, or <paramref name="value"/>.</returns>
        public TValue OrElse(TValue value) => OrElseGet(() => value);
        
        /// <summary>
        ///     Returns the current optional, or an empty one if the result of <paramref name="predicate"/> is <c>false</c>.
        /// </summary>
        /// <param name="predicate">The async predicate function to test the value.</param>
        /// <returns>An optional, containing no value if <paramref name="predicate"/> returns <c>false</c>.</returns>
        public async Task<Possible<TValue>> OnlyIfAsync([NotNull] Func<TValue, Task<bool>> predicate)
            => _value != null && await predicate(OrNull()) ? this : new Possible<TValue>();

        /// <summary>
        ///     Coerces the current <see cref="Possible{TValue}"/>'s value into a value of <typeparamref name="T"/>. 
        /// </summary>
        /// <param name="transformation">The async transformation function.</param>
        /// <typeparam name="T">The type of the new <see cref="Possible{TValue}"/>.</typeparam>
        /// <returns>The new <see cref="Possible{TValue}"/>.</returns>
        public async Task<Possible<T>> TransformAsync<T>([NotNull] Func<TValue, Task<T>> transformation)
            where T : class
            => _value != null
                ? new Possible<T>(await transformation(OrNull()))
                : new Possible<T>();

        /// <summary>
        ///     Coerces the current <see cref="Possible{TValue}"/>'s value into a value of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="transformation">The async transformation function.</param>
        /// <typeparam name="T">The type of the new <see cref="Possible{TValue}"/>.</typeparam>
        /// <returns>The new <see cref="Possible{TValue}"/>.</returns>
        public async Task<Possible<T>> TransformAsync<T>([NotNull] Func<TValue, Task<Possible<T>>> transformation)
            where T : class
            => _value != null
                ? new Possible<T>(await transformation(OrNull()))
                : new Possible<T>();

        /// <summary>
        ///     If this <see cref="Possible{TValue}"/>'s value is not null, <paramref name="action"/> is run.
        /// </summary>
        /// <param name="action">The async action to perform.</param>
        /// <returns>The current <see cref="Possible{TValue}"/>.</returns>
        public Possible<TValue> IfPresentAsync([NotNull] Func<TValue, Task> action) {
            if (OrNull() != null)
                action(OrNull());
            return this;
        }

        /// <summary>
        ///     Returns the value of this <see cref="Possible{TValue}"/>.
        ///     If the value is null, the return value of the function <paramref name="exceptionProducer"/> is thrown.
        /// </summary>
        /// <param name="exceptionProducer">The function that produces the exception to throw when this <see cref="Possible{TValue}"/>'s value is null.</param>
        /// <returns>The value of this <see cref="Possible{TValue}"/>.</returns>
        public async Task<TValue> OrThrowAsync([NotNull] Func<Task<Exception>> exceptionProducer) =>
            OrThrow(await exceptionProducer());

        /// <summary>
        ///     Returns the value of this <see cref="Possible{TValue}"/>.
        ///     If the value is null, the value of <paramref name="valueProducer"/> is returned instead.
        /// </summary>
        /// <param name="valueProducer">The value to return when this <see cref="Possible{TValue}"/>'s value is null.</param>
        /// <returns>The value of this <see cref="Possible{TValue}"/>, or the result of <paramref name="valueProducer"/>.</returns>
        public async Task<TValue> OrProduceAsync([NotNull] Func<Task<TValue>> valueProducer) =>
            OrNull() ?? await valueProducer();
        
        /// <summary>
        ///     Returns the value of this <see cref="Possible{TValue}"/>.
        ///     If the value is null, the value of <paramref name="valueProducer"/> is returned instead.
        /// </summary>
        /// <param name="valueProducer">The value to return when this <see cref="Possible{TValue}"/>'s value is null.</param>
        /// <returns>The value of this <see cref="Possible{TValue}"/>, or the result of <paramref name="valueProducer"/>.</returns>
        public async Task<TValue> OrElseGetAsync([NotNull] Func<Task<TValue>> valueProducer) =>
            OrNull() ?? await valueProducer();

        /// <summary>
        ///     Returns the nullable value of this <see cref="Possible{TValue}"/>.
        /// </summary>
        /// <returns>The value of this <see cref="Possible{TValue}"/>.</returns>
        public TValue OrNull() => _value;

        public static implicit operator Possible<TValue>([CanBeNull] TValue value) => new Possible<TValue>(value);

        [CanBeNull]
        public static implicit operator TValue(Possible<TValue> value) => value.OrNull();

        public static bool operator ==(Possible<TValue> left, [CanBeNull] object right) => left.Equals(right);
        public static bool operator !=(Possible<TValue> left, [CanBeNull] object right) => !(left == right);

        public static Possible<TValue> operator +(Possible<TValue> left, [CanBeNull] object right) {
            left._value = right.Cast<TValue>();
            return left;
        }

        // literally who fucking cares
        // ReSharper disable NonReadonlyMemberInGetHashCode
        public override int GetHashCode() => _value != null ? _value.GetHashCode() : -1;

        public override bool Equals(object obj) {
            var trueValue = obj is Possible<TValue> opt
                ? opt._value
                : obj;

            if (_value is null && trueValue is null)
                return true;
            if (trueValue != null && _value is null || trueValue is null && _value != null)
                return false;

            return _value.Equals(trueValue);
        }

        public override string ToString() => _value?.ToString();

        public object Clone() => new Possible<TValue>(_value);

        public void Dispose() {
            // while this *can* be a no-op method, it *does* allow for the
            // disposable pattern for any IDisposable objects you may use with this type.
            // you can even use the disposable pattern for non-disposable types,
            // due to this class itself implementing IDisposable.
            if (_value is IDisposable disposable)
                disposable.Dispose();
        }
    }
}