using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Decor.UnitTests
{
    public class DependencyInjectionTests
    {
        [Fact]
        public void Method_WithDecoratorWithDependencies_ShouldInjectDependencies()
        {
            // Arrange
            var services = GetServices();
            var sut = services.GetService<SomeClass>();

            // Act
            sut.Method();

            // Assert
            var decorator = services.GetService<TestDecorator>();
            var expectedDependency = services.GetService<SomeDependency>();
            decorator.CallCount.Should().Be(1);
            decorator.SomeDependency.Should().Be(expectedDependency);
        }

        #region Setup
        public class SomeDependency { }

        public class TestDecorator : IDecorator
        {
            public TestDecorator(SomeDependency someDependency) { SomeDependency = someDependency; }

            public SomeDependency SomeDependency { get; }

            public int CallCount { get; set; }

            public async Task OnInvoke(Call call) { CallCount++; await call.Next(); }
        }

        public class SomeClass
        {
            [Decorate(typeof(TestDecorator))]
            virtual public void Method() { }
        }

        private IServiceProvider GetServices()
            => new ServiceCollection()
                .AddDecor()
                .AddSingleton<SomeDependency>()
                .AddSingleton<TestDecorator>()
                .AddTransientDecorated<SomeClass>()
                .BuildServiceProvider();
        #endregion
    }
}
