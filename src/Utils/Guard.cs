using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Contract = JetBrains.Annotations.ContractAnnotationAttribute;

namespace Gommon {
    /// <summary>
    ///     Provides common value check methods, such as nullability, collection length, and conditions.
    /// </summary>
    public class Guard {
        private Guard() { }

        [Contract("null => halt")]
        public static void Incomplete([CanBeNull] Task task) {
            Require(task, "Task");
            Ensure(!task.IsCompleted, "Task is already completed, successfully or unsuccessfully.");
        }

        [Contract("null => halt")]
        public static void Completed([CanBeNull] Task task) {
            Require(task, "Task");
            Ensure(task.IsCompleted, "Task is not completed, successfully or unsuccessfully.");
        }

        [Contract("null => halt")]
        public static void ValidSnowflake([CanBeNull] string snowflake) => ValidSnowflake(snowflake, $"{snowflake}");

        public static void ValidSnowflake([CanBeNull] string snowflake, [NotNull] string message) {
            Require(snowflake, message);
            var nw = snowflake.Replace(" ", "");
            if (nw.Length > 20 || !nw.All(char.IsNumber))
                ValueException.Throw($"{message} is not a valid snowflake string! Provided: \"{snowflake}\"");
        }

        [Contract("false => halt")]
        public static void Ensure(bool expression) {
            if (!expression)
                ValueException.Throw("Expression does not equal true.");
        }

        [Contract("expression:false => halt")]
        public static void Ensure(bool expression, [NotNull] string message) {
            if (!expression)
                ValueException.Throw(message);
        }

        [Contract("expression:false => halt")]
        public static void Ensure(bool expression, [NotNull] string message, [ItemCanBeNull] params object[] args)
            => Ensure(expression, message.Format(args));

        [Contract("input:null => halt; regex:null => halt")]
        public static void Matches([NotNull] string input, [NotNull] string regex, [CanBeNull] string name = null) {
            Require(input, name);
            Require(regex, name ?? "Regular expression");
            Ensure(new Regex(regex).IsMatch(input),
                $"{name ?? "Input"} must match regex ^{regex}$. Provided: \"{input}\"");
        }

        public static void NotEmpty<T>(IEnumerable<T> coll, string message, params object[] args)
            => NotEmpty(coll, message.Format(args));

        public static void NotEmpty<T>(IEnumerable<T> coll, string message) {
            if (coll.None())
                ValueException.Throw(message);
        }

        public static void NoneNull<T>(IEnumerable<T> coll, string message, params object[] args)
            => NoneNull(coll, message.Format(args));

        public static void NoneNull<T>(IEnumerable<T> coll, string message) {
            if (coll.Any(x => x is null))
                ValueException.Throw(message);
        }

        [Contract("null => halt")]
        public static void Require(object value) => Require(value, null);

        [Contract("value:null => halt")]
        public static void Require(object value, string name) {
            value.AsOptional().OrThrow(() => new ValueException($"{name ?? "Value"} must not be null."));
        }
    }

    public class ValueException : Exception {
        [Contract("=> halt")]
        public static void Throw(string message = null, Exception innerException = null)
            => throw new ValueException(message, innerException);

        public ValueException(string message = null, Exception innerException = null) :
            base(message, innerException) { }
    }
}