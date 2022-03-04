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
            var decorator = services.GetService<TestDecoratorWithDependencies>();
            var expectedDependency = services.GetService<SomeDependency>();
            decorator.CallCount.Should().Be(1);
            decorator.SomeDependency.Should().Be(expectedDependency);
        }

        [Fact]
        public void Method_WithClassWithDependencies_ShouldInjectDependencies()
        {
            // Arrange
            var services = GetServices();
            var sut = services.GetService<ISomeClassWithDependencies>();

            // Act
            sut.Method();

            // Assert
            var decorator = services.GetService<TestDecoratorWithDependencies>();
            var expectedDependency = services.GetService<SomeDependency>();
            decorator.CallCount.Should().Be(1);
            sut.SomeDependency.Should().Be(expectedDependency);
        }

        #region Setup
        public class SomeDependency { }

        public class TestDecoratorWithDependencies : IDecorator
        {
            public TestDecoratorWithDependencies(SomeDependency someDependency) { SomeDependency = someDependency; }

            public SomeDependency SomeDependency { get; }

            public int CallCount { get; set; }

            public async Task OnInvoke(Call call) { CallCount++; await call.Next(); }
        }

        public class SomeClass
        {
            [Decorate(typeof(TestDecoratorWithDependencies))]
            virtual public void Method() { }
        }

        public interface ISomeClassWithDependencies
        {
            SomeDependency SomeDependency { get; }
            void Method();
        }

        public class SomeClassWithDependencies : ISomeClassWithDependencies
        {
            public SomeClassWithDependencies(SomeDependency someDependency)
            {
                SomeDependency = someDependency;
            }

            public SomeDependency SomeDependency { get; }

            [Decorate(typeof(TestDecoratorWithDependencies))]
            virtual public void Method() { }
        }

        private IServiceProvider GetServices()
            => new ServiceCollection()
                .AddDecor()
                .AddSingleton<SomeDependency>().Decorated()
                .AddSingleton<TestDecoratorWithDependencies>()
                .AddTransient<SomeClass>().Decorated()
                .AddTransient<ISomeClassWithDependencies, SomeClassWithDependencies>().Decorated()
                .BuildServiceProvider();
        #endregion
    }
}
