using Microsoft.Extensions.DependencyInjection;
using System;

namespace Decor.Extensions.Microsoft.DependencyInjection.Internal
{
    public class DependencyInjectionDecoratorProvider : IDecoratorProvider
    {
        private IServiceProvider _services;

        public DependencyInjectionDecoratorProvider(IServiceProvider services)
        {
            _services = services;
        }

        public IDecorator Get(Type decoratorType)
        {
            return (IDecorator)_services.GetRequiredService(decoratorType);
        }
    }
}
