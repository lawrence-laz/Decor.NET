using System;

namespace Decor.Internal
{
    public class ActivatorDecoratorProvider : IDecoratorProvider
    {
        public IDecorator Get(Type decoratorType)
        {
            return (IDecorator)Activator.CreateInstance(decoratorType);
        }
    }
}
