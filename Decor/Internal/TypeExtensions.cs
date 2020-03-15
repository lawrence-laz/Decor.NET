using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Decor.Internal
{
    internal static class TypeExtensions
    {
        private static readonly Type _voidTaskResultType = Type.GetType("System.Threading.Tasks.VoidTaskResult", false);

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

        internal static bool IsAsync(this MethodInfo method)
            => method.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;

        internal static bool IsTaskWithVoidTaskResult(this Type type)
            => type.GenericTypeArguments?.Length > 0 && type.GenericTypeArguments[0] == _voidTaskResultType;

        internal static bool TryGetGenericTaskType(this TypeInfo type, out TypeInfo genericTaskType)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            while (type != null)
            {
                if (IsGenericTaskType(type))
                {
                    genericTaskType = type;
                    return true;
                }

                type = type.BaseType.GetTypeInfo();
            }

            genericTaskType = null;
            return false;

            bool IsGenericTaskType(TypeInfo typeInfo) => typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Task<>);
        }
    }
}
