using System;
using System.Reflection;

namespace Gommon
{
    public static class ReflectionExtensions
    {
        /// <summary>
        ///     Checks whether or not the current <see cref="MemberInfo"/> has the given <typeparam name="T"/> attribute.
        /// </summary>
        /// <typeparam name="T">Type of the attribute in question</typeparam>
        /// <param name="member">Current <see cref="MemberInfo"/></param>
        /// <returns><see cref="bool"/></returns>
        public static bool HasAttribute<T>(this MemberInfo member) where T : Attribute
            => member.GetCustomAttribute<T>() != null;

        /// <summary>
        ///     Checks whether or not the current <see cref="Type"/> has the given <typeparam name="T"/> attribute.
        /// </summary>
        /// <typeparam name="T">Type of the attribute in question</typeparam>
        /// <param name="type">Current <see cref="Type"/></param>
        /// <returns><see cref="bool"/></returns>
        public static bool HasAttribute<T>(this Type type) where T : Attribute
            => type.GetCustomAttribute<T>() != null;
    }
}
