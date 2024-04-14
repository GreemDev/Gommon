using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

// ReSharper disable MemberCanBePrivate.Global
namespace Gommon
{
    public static partial class Extensions
    {
        /// <summary>
        ///     Checks whether or not the current <see cref="MemberInfo"/> has the given <typeparam name="T"/> attribute.
        /// </summary>
        /// <typeparam name="T">Type of the attribute in question</typeparam>
        /// <param name="memberInfo">Current <see cref="MemberInfo"/>.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool HasAttribute<T>(this MemberInfo memberInfo) where T : Attribute
            => memberInfo.GetCustomAttribute<T>() != null;

        /// <summary>
        ///     Checks whether or not the current <see cref="Type"/> has the given <typeparam name="T"/> attribute, and if it does, it is provided via an out parameter.
        /// </summary>
        /// <typeparam name="T">Type of the attribute in question</typeparam>
        /// <param name="memberInfo">Current <see cref="MemberInfo"/>.</param>
        /// <param name="attribute">The possibly-null attribute.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool TryGetAttribute<T>(this MemberInfo memberInfo, [NotNullWhen(true)] out T attribute)
            where T : Attribute
        {
            attribute = null;

            if (memberInfo.HasAttribute<T>())
                attribute = memberInfo.GetCustomAttribute<T>();

            return attribute != null;
        }

        /// <summary>
        ///     Checks whether or not the current <see cref="Type"/> inherits/implements the given <typeparamref name="T"/>, whether that be a class or an interface.
        ///     Note: this also checks implicit type conversions, so it may not be 100% correct for actual type bases!
        /// </summary>
        /// <typeparam name="T">The type to compare with</typeparam>
        /// <param name="type">The type to check for inheritance</param>
        /// <returns>Whether or not this type inherits (or can be implicitly converted to) the given type <typeparamref name="T"/>.</returns>
        public static bool Inherits<T>(this Type type)
            => typeof(T).IsAssignableFrom(type);


        /// <summary>
        ///     Formats a type to a pretty .NET-styled type name.
        /// </summary>
        /// <param name="type">The current Type.</param>
        /// <param name="languageCorrectPrimitives">Whether to convert type names to their respective name in C#; for example, System.String becomes string.</param>
        /// <returns>A pretty string that shows what this type is, removing the rather ugly style of the regular Type#ToString() result.</returns>
        public static string AsPrettyString(this Type type, bool languageCorrectPrimitives = true)
        {
            var nullableTypeRegex = new Regex(@"Nullable<(?<T>.+)>", RegexOptions.Compiled);

            var types = type.GenericTypeArguments;

            //thanks .NET for putting an annoying ass backtick and number at the end of type names.
            var vs = formatTypeName(type.Name).Replace($"`{types.Length}", "");

            if (!types.None()) vs += $"<{types.Select(x => x.AsPrettyString(languageCorrectPrimitives)).Select(formatTypeName).JoinToString(", ")}>";

            if (nullableTypeRegex.IsMatch(vs, out var match))
                vs = $"{match.Groups["T"].Value}?";
            
            return vs;
            
            string formatTypeName(string typeName) =>
                !languageCorrectPrimitives
                    ? typeName
                    : typeName switch
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
        }

        /// <summary>
        ///     Formats a type to a pretty .NET-styled type name, including the full namespace of the type prepended at the beginning.
        /// </summary>
        /// <param name="type">The current Type.</param>
        /// <returns>A pretty string that shows the full name of this type, and removing the rather ugly style of the regular Type#ToString() result.</returns>
        public static string AsFullNamePrettyString(this Type type)
            => $"{type.Namespace}.{type.AsPrettyString(false)}";

        /// <summary>
        ///     Checks if the current <see cref="MethodInfo"/>'s parameter count, order, and types match that of <paramref name="paramTypes"/>.
        /// </summary>
        /// <param name="method">The current method.</param>
        /// <param name="paramTypes">The arguments, in order and count, desired by the caller of this function.</param>
        /// <returns>A tuple containing a boolean and a nullable string. <br/>If the signature matches, the boolean will be true and the string will be null. If it does not match, the Error string has a value.</returns>
        public static (bool Matches, string Error) CheckSignature(this MethodInfo method, params Type[] paramTypes)
        {
            var parameters = method.GetParameters();

            if (parameters.Length != paramTypes.Length)
                return (false, $"Method needs {paramTypes.Length} parameters, has {parameters.Length} parameters!");

            foreach (var (param, i) in parameters.WithIndex())
            {
                var actualFullName = param.ParameterType.AsFullNamePrettyString();
                var requiredFullName = paramTypes[i].AsFullNamePrettyString();
                if (actualFullName != requiredFullName)
                    return (false,
                        $"Parameter at index {i} is not of the required type! Required: {requiredFullName}, Actual: {actualFullName}");
            }

            return (true, null);
        }
    }
}