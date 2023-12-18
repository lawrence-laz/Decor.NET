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

        public ReadOnlyDictionary<MethodInfo, Type[]> Get(Type decoratedType, Func<MethodInfo, Type[]> action = null) => _map.GetOrAdd(decoratedType, Factory(decoratedType, action));

        private static ReadOnlyDictionary<MethodInfo, Type[]> Factory(Type decoratedType, Func<MethodInfo, Type[]> methodSelectorAction = null)
        {
            var map = new Dictionary<MethodInfo, Type[]>();

            var methods = decoratedType.GetDecoratableMethods();

            foreach (var method in methods)
            {
                if (methodSelectorAction != null)
                {
                    var actionResult = methodSelectorAction(method);
                    if (actionResult.Any())
                    {
                        map.Add(method, actionResult);
                    }
                }
                else
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
            }

            return new ReadOnlyDictionary<MethodInfo, Type[]>(map);
        }
    }
}
