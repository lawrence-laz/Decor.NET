using System;

namespace Decor
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class DecorateAttribute : Attribute
    {
        public Type DecoratorType { get; }

        public DecorateAttribute(Type decoratorType)
        {
            if (!typeof(IDecorator).IsAssignableFrom(decoratorType))
            {
                throw new ArgumentException($"Type '{decoratorType.Name}' does not implement the interface '{nameof(IDecorator)}'", nameof(decoratorType));
            }

            DecoratorType = decoratorType;
        }
    }
}
