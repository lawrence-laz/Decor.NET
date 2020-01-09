using Castle.DynamicProxy;
using Decor.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Decor
{
    public sealed class Decorator
    {
        public IDecoratorProvider DecoratorProvider { get; set; }

        private ProxyGenerator _proxyGenerator = new ProxyGenerator();

        public Decorator()
        {
            DecoratorProvider = new ActivatorDecoratorProvider();
        }

        public Decorator(IDecoratorProvider decoratorProvider)
        {
            DecoratorProvider = decoratorProvider ?? new ActivatorDecoratorProvider();
        }

        public TInterface For<TInterface, TImplementation>(TImplementation implementation) where TImplementation : TInterface
        {
            if (implementation == null)
            {
                return implementation;
            }

            var interceptors = GetInterceptors(implementation);

            var proxy = _proxyGenerator.CreateInterfaceProxyWithTargetInterface(
                typeof(TInterface), implementation, interceptors);

            return (TInterface)proxy;
        }

        public TImplementation For<TImplementation>(TImplementation implementation)
        {
            if (implementation == null)
            {
                return implementation;
            }

            var interceptors = GetInterceptors(implementation);

            var proxy = _proxyGenerator.CreateClassProxyWithTarget(typeof(TImplementation), implementation,
                interceptors);

            return (TImplementation)proxy;
        }

        private IInterceptor[] GetInterceptors(object implementation)
        {
            var methodsAndAttributes = implementation.GetType().GetAllMethods().Select(m =>
                    (Method: m,
                     Attributes: m.GetCustomAttributes(typeof(DecorateAttribute), true).Cast<DecorateAttribute>()))
                .Where(x => x.Attributes.Any());

            var attributes = methodsAndAttributes.SelectMany(x => x.Attributes).Distinct();
            var interceptors = new List<AttributeInterceptor>();
            foreach (var attribute in attributes)
            {
                var methods = new HashSet<MethodInfo>(methodsAndAttributes
                    .Where(x => x.Attributes.Contains(attribute))
                    .Select(x => x.Method).Distinct());

                var interceptor = new AttributeInterceptor(DecoratorProvider.Get(attribute.DecoratorType), methods);

                interceptors.Add(interceptor);
            }

            return interceptors.Select(x => x.ToInterceptor()).ToArray();
        }
    }
}
