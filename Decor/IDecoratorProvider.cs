using Decor.Internal;
using System;

namespace Decor
{
    public interface IDecoratorProvider
    {
        IBaseDecorator Get(Type decoratorType);
    }
}
