using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Gommon.Extensions
{
    public static class ReflectionExtensions
    {
        public static bool HasAttribute<T>(this MemberInfo member) where T : Attribute
            => member.GetCustomAttribute<T>() != null;

        public static bool HasAttribute<T>(this Type type) where T : Attribute
            => type.GetCustomAttribute<T>() != null;
    }
}
