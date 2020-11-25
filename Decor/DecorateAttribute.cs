using System;

namespace Decor
{
    /// <summary>
    /// <para>Marks a method to be intercepted by a given <see cref="IDecorator"/>.</para>
    /// <para>Can be used in both interface and class methods within a hierarchy.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class DecorateAttribute : Attribute
    {
        /// <summary>
        /// Gets the <see cref="Type"/> represting the registered decorator.
        /// </summary>
        public Type DecoratorType { get; }

        /// <summary>
        /// Creates an attribute decorating method with a single type of <see cref="IDecorator"/>.
        /// </summary>
        /// <param name="decoratorType">A <see cref="Type"/> assignable to <see cref="IDecorator"/>.</param>
        public DecorateAttribute(Type decoratorType)
        {
            if (decoratorType == null)
            {
                throw new ArgumentNullException(nameof(decoratorType));
            }

            if (!typeof(IDecorator).IsAssignableFrom(decoratorType))
            {
                throw new ArgumentException(
                    $"Type '{decoratorType.Name}' does not implement interface '{nameof(IDecorator)}'", nameof(decoratorType));
            }

            DecoratorType = decoratorType;
        }
    }
}
