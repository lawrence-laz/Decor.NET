using System;
using System.Collections.Generic;
using System.Reflection;

namespace Decor.Internal
{
    public static class TypeExtensions
    {
        internal static IEnumerable<MethodInfo> GetAllMethods(this Type type)
        {
            var methods = new List<MethodInfo>();
            while (type != null)
            {
                methods.AddRange(type.GetMethods());

                foreach (var @interface in type.GetInterfaces())
                {
                    methods.AddRange(@interface.GetMethods());
                }

                methods.AddRange(type.GetMethods());
                type = type.BaseType;
            }

            return methods;
        }
    }
}
