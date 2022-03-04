using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Decor.UnitTests
{
    public class SingleDecoratorInstanceTests
    {
        [Fact]
        public void Method_CalledFirstTime_ShouldTakeLessThan300Ms()
        {
            // Arrange
            var services = GetServices();
            var decoratedObject = services.GetService<ISomeInterface>();

            // Act
            var actual1 = decoratedObject.Method1();
            var actual2 = decoratedObject.Method2();

            // Assert
            actual1.Should().Be(actual2);
        }

        #region Setup
        public class TestDecorator : IDecorator
        {
            public async Task OnInvoke(Call call)
            {
                await call.Next();
                call.ReturnValue = this;
            }
        }

        public interface ISomeInterface
        {
            [Decorate(typeof(TestDecorator))]
            IDecorator Method1();

            [Decorate(typeof(TestDecorator))]
            IDecorator Method2();
        }

        public class SomeClass : ISomeInterface
        {
            public IDecorator Method1() => default;
            public IDecorator Method2() => default;
        }

        private IServiceProvider GetServices()
            => new ServiceCollection()
                .AddDecor()
                .AddTransient<TestDecorator>()
                .AddTransientDecorated<ISomeInterface, SomeClass>()
                .BuildServiceProvider();
        #endregion
    }
}
