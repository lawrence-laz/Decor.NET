using System;

namespace Decor.Internal
{
    public class ActivatorDecoratorProvider : IDecoratorProvider
    {
        public IBaseDecorator Get(Type decoratorType)
        {
            return (IBaseDecorator)Activator.CreateInstance(decoratorType);
        }
    }
}
