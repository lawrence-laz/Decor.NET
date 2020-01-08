using System;

namespace Decor
{
    public interface IDecoratorProvider
    {
        IDecorator Get(Type decoratorType);
    }
}
