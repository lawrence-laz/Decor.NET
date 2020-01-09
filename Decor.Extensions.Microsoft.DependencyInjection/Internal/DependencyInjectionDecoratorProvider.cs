using Decor.Internal;
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

        public IBaseDecorator Get(Type decoratorType)
        {
            return (IBaseDecorator)_services.GetRequiredService(decoratorType);
        }
    }
}
