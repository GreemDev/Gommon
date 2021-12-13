using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Gommon {
    public static partial class Extensions {
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
            where T : Attribute {
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
    }
}