using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gommon
{
    public partial class Extensions
    {
        private const int MemoryTierSize = 1024;
        
        /// <summary>
        ///     Formats a type to a pretty .NET-styled type name.
        /// </summary>
        /// <param name="type">The current Type.</param>
        /// <returns>A pretty string that shows what this type is.</returns>
        public static string AsPrettyString(this Type type)
        {
            string FormatTypeName(string typeName)
            {
                switch (typeName)
                {
                    case "Boolean": return "bool";
                    case "Byte": return "byte";
                    case "SByte": return "sbyte";
                    case "Int16": return "short";
                    case "UInt16": return "ushort";
                    case "Int32": return "int";
                    case "UInt32": return "uint";
                    case "Int64": return "long";
                    case "UInt64": return "ulong";
                    case "Char": return "char";
                    case "String": return "string";
                    default: return typeName;
                }
            }
            
            string FormatType(Type t)
            {
                return FormatTypeName(t.Name);
            }

            var types = type.GenericTypeArguments;

            //thanks .NET for putting an annoying ass backtick and number at the end of type names.
            var vs = FormatType(type).Replace($"`{types.Length}", "");

            if (!types.IsEmpty()) vs += $"<{types.Select(x => x.AsPrettyString()).Select(FormatTypeName).Join(", ")}>";

            return vs;
        }
        
        /// <summary>
        ///     Gets all flags in the current enum.
        /// </summary>
        /// <param name="input">The current enum.</param>
        /// <typeparam name="T">The enum's type.</typeparam>
        /// <returns>A collection of all the current Enum's members.</returns>
        public static IEnumerable<T> GetFlags<T>(this T input) where T : Enum
        {
            return Enumerable.Cast<T>(Enum.GetValues(input.GetType())).Where(e => input.HasFlag(e));
        }

        /// <summary>
        ///     Appends all elements in the specified string array as a line to the current StringBuilder.
        /// </summary>
        /// <param name="sb">The current StringBuilder.</param>
        /// <param name="lines">The lines to append.</param>
        /// <returns>The current StringBuilder for chaining convenience.</returns>
        public static StringBuilder AppendAllLines(this StringBuilder sb, params string[] lines)
        {
            foreach (var line in lines)
            {
                sb.AppendLine(line);
            }

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
        {
            var res = process.PrivateMemorySize64;
            switch (memType)
            {
                case MemoryType.Terabytes:
                    return $"{res / MemoryTierSize / MemoryTierSize / MemoryTierSize / MemoryTierSize} TB";
                case MemoryType.Gigabytes: return $"{res / MemoryTierSize / MemoryTierSize / MemoryTierSize} GB";
                case MemoryType.Megabytes: return $"{res / MemoryTierSize / MemoryTierSize} MB";
                case MemoryType.Kilobytes: return $"{res / MemoryTierSize} KB";
                case MemoryType.Bytes: return $"{res} B";
                default: return "null";
            }
        }
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