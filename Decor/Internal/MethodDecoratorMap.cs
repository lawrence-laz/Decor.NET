using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Decor.Internal
{
    internal class MethodDecoratorMap
    {
        private readonly ConcurrentDictionary<Type, ReadOnlyDictionary<MethodInfo, Type[]>> _map;

        public MethodDecoratorMap() => _map = new ConcurrentDictionary<Type, ReadOnlyDictionary<MethodInfo, Type[]>>();

        public ReadOnlyDictionary<MethodInfo, Type[]> Get(Type decoratedType) => _map.GetOrAdd(decoratedType, Factory);

        private ReadOnlyDictionary<MethodInfo, Type[]> Factory(Type decoratedType)
        {
            var map = new Dictionary<MethodInfo, Type[]>();

            var methods = decoratedType.GetDecoratableMethods();

            foreach (var method in methods)
            {
                var decoratorAttributes = method.GetCustomAttributes<DecorateAttribute>(true);
                var interfaceMethod = method.GetInterfaceMethod();
                if (interfaceMethod != null)
                {
                    decoratorAttributes = decoratorAttributes.Concat(interfaceMethod.GetCustomAttributes<DecorateAttribute>(true));
                }

                var decorators = decoratorAttributes.Select(attribute => attribute.DecoratorType).Distinct().ToArray();

                map.Add(method, decorators);
            }

            return new ReadOnlyDictionary<MethodInfo, Type[]>(map);
        }
    }
}
