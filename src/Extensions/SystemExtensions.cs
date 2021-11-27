using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Gommon
{
    public partial class Extensions
    {
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
        public static bool TryGet<T>(this IServiceProvider serviceProvider, out T service)
        {
            service = serviceProvider.Get<T>();
            return service != null;
        }

        /// <summary>
        ///     Run the specified <see cref="Action{T}"/> with <paramref name="curr"/> passed in as its value.
        /// </summary>
        /// <param name="curr">The current object.</param>
        /// <param name="apply">The action to perform.</param>
        /// <typeparam name="T">The type of the current object.</typeparam>
        /// <returns><paramref name="curr"/> after <paramref name="apply"/> runs on it.</returns>
        public static T Apply<T>(this T curr, Action<T> apply)
        {
            apply(curr);
            return curr;
        }
        
        public static T ValueLock<T>(this object @lock, Func<T> action)
        {
            lock (@lock)
                return action();
        }

        public static void Lock(this object @lock, Action action)
        {
            lock (@lock)
                action();
        }

        public static void LockedRef<T>(this T obj, Action<T> action)
        {
            lock (obj)
                action(obj);
        }

        private const int _memoryTierSize = 1024;

        /// <summary>
        ///     Formats a type to a pretty .NET-styled type name.
        /// </summary>
        /// <param name="type">The current Type.</param>
        /// <returns>A pretty string that shows what this type is, removing the rather ugly style of the regular Type#ToString() result.</returns>
        public static string AsPrettyString(this Type type)
        {
            var regex = new Regex(@"Nullable<(?<T>.+)>", RegexOptions.Compiled);
            
            string _formatTypeName(string typeName) 
                => typeName switch
                {
                    "Boolean" => "bool",
                    "Single" => "float",
                    "Decimal" => "decimal",
                    "Byte" => "byte",
                    "SByte" => "sbyte",
                    "Int16" => "short",
                    "UInt16" => "ushort",
                    "Int32" => "int",
                    "UInt32" => "uint",
                    "Int64" => "long",
                    "UInt64" => "ulong",
                    "Char" => "char",
                    "String" => "string",
                    _ => typeName
                };
            

            var types = type.GenericTypeArguments;

            //thanks .NET for putting an annoying ass backtick and number at the end of type names.
            var vs = _formatTypeName(type.Name).Replace($"`{types.Length}", "");

            if (!types.None()) vs += $"<{types.Select(x => x.AsPrettyString()).Select(_formatTypeName).Join(", ")}>";

            if (regex.IsMatch(vs, out var match))
            {
                var typeName = match.Groups["T"].Value;
                vs = $"{typeName}?";
            }

            return vs;
        }

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
        public static StringBuilder AppendAllLines(this StringBuilder sb, params string[] lines)
        {
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
        public static bool IsMatch(this Regex regex, string str, out Match match)
        {
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
            => memType switch
            {
                MemoryType.Terabytes =>
                    $"{process.PrivateMemorySize64 / _memoryTierSize / _memoryTierSize / _memoryTierSize / _memoryTierSize} TB",
                MemoryType.Gigabytes =>
                    $"{process.PrivateMemorySize64 / _memoryTierSize / _memoryTierSize / _memoryTierSize} GB",
                MemoryType.Megabytes => 
                    $"{process.PrivateMemorySize64 / _memoryTierSize / _memoryTierSize} MB",
                MemoryType.Kilobytes => 
                    $"{process.PrivateMemorySize64 / _memoryTierSize} KB",
                MemoryType.Bytes => 
                    $"{process.PrivateMemorySize64} B",
                _ => "null"
            };
    }

    public enum MemoryType
    {
        Terabytes,
        Gigabytes,
        Megabytes,
        Kilobytes,
        Bytes
    }
}