using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Decor.Internal
{
    internal static class TypeExtensions
    {
        internal static IEnumerable<MethodInfo> GetDecoratableMethods(this Type type)
            => type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(x => !x.IsSpecialName);

        internal static MethodInfo GetInterfaceMethod(this MethodInfo implementation)
        {
            var interfaces = implementation.DeclaringType.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                var map = implementation.DeclaringType.GetInterfaceMap(@interface);
                var index = Array.IndexOf(map.TargetMethods, implementation);

                if (index != -1)
                {
                    return map.InterfaceMethods[index];
                }
            }

            return null;
        }

        internal static bool IsAsync(this MethodInfo method) => method.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;
    }
}
