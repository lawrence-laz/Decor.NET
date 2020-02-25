using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Decor.UnitTests
{
    public class CallOrderTests
    {
        [Fact]
        public void Method_WithAscendingDecorators_ShouldCallInAscendingOrder()
        {
            // Arrange
            var services = GetServices();
            var someClass = services.GetService<SomeClass>();
            var decorator1 = services.GetService<Decorator1>();
            var decorator2 = services.GetService<Decorator2>();

            // Act
            someClass.AscendingMethod();

            // Assert
            decorator1.CallTime.Should().BeBefore(decorator2.CallTime);
        }

        [Fact]
        public void Method_WithDescendingDecorators_ShouldCallInDescendingOrder()
        {
            // Arrange
            var services = GetServices();
            var someClass = services.GetService<SomeClass>();
            var decorator1 = services.GetService<Decorator1>();
            var decorator2 = services.GetService<Decorator2>();

            // Act
            someClass.DescendingMethod();

            // Assert
            decorator1.CallTime.Should().BeAfter(decorator2.CallTime);
        }

        #region Setup
        public class Decorator1 : IDecorator
        {
            public DateTime CallTime { get; set; }

            public async Task OnInvoke(Call call)
            {
                CallTime = DateTime.UtcNow;
                await call.Next();
            }
        }

        public class Decorator2 : IDecorator
        {
            public DateTime CallTime { get; set; }

            public async Task OnInvoke(Call call)
            {
                CallTime = DateTime.UtcNow;
                await call.Next();
            }
        }

        public class SomeClass
        {
            [Decorate(typeof(Decorator1))]
            [Decorate(typeof(Decorator2))]
            public virtual void AscendingMethod() { }

            [Decorate(typeof(Decorator2))]
            [Decorate(typeof(Decorator1))]
            public virtual void DescendingMethod() { }
        }

        private IServiceProvider GetServices()
            => new ServiceCollection()
                .AddDecor()
                .AddSingleton<Decorator1>()
                .AddSingleton<Decorator2>()
                .AddTransientDecorated<SomeClass>()
                .BuildServiceProvider();
        #endregion
    }
}
