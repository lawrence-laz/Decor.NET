using AutoFixture.Xunit2;
using Decor.UnitTests.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace Decor.UnitTests
{
    public class DecoratorTests
    {
        [Fact]
        public void Method_WithAttributeInClass_ShouldCallDecorMethods()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<SomeDecorator>();
            var sut = services.GetService<ISomeInterface>();

            // Act
            sut.AttributeInClassMethod();

            // Assert
            Assert.Equal(1, decorator.OnAfterCallCount);
            Assert.Equal(1, decorator.OnBeforeCallCount);
        }

        [Fact]
        public void Method_WithAttributeInInterface_ShouldCallDecorMethods()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<SomeDecorator>();
            var sut = services.GetService<ISomeInterface>();

            // Act
            sut.AttributeInInterfaceMethod();

            // Assert
            Assert.Equal(1, decorator.OnAfterCallCount);
            Assert.Equal(1, decorator.OnBeforeCallCount);
        }

        [Fact]
        public async Task AsyncMethod_WithAttributeInInterface_ShouldCallDecorMethods()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<SomeDecorator>();
            var sut = services.GetService<ISomeInterface>();
            var stopwatch = Stopwatch.StartNew();

            // Act
            var result = await sut.AsyncAttributeInInterfaceMethod(stopwatch);

            // Assert
            Assert.True(result < stopwatch.ElapsedMilliseconds, "OnAfter was called before method completed.");
            Assert.Equal(1, decorator.OnAfterCallCount);
            Assert.Equal(1, decorator.OnBeforeCallCount);
        }

        [Fact]
        public void Method_WithNoAttribute_ShouldNotCallDecorMethods()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<SomeDecorator>();
            var sut = services.GetService<ISomeInterface>();

            // Act
            sut.NoAttributeMethod();

            // Assert
            Assert.Equal(0, decorator.OnAfterCallCount);
            Assert.Equal(0, decorator.OnBeforeCallCount);
        }

        [Fact]
        public void Method_WithMultipleAttributes_ShouldCallAllDecorMethods()
        {
            // Arrange
            var services = GetServices();
            var decorator1 = services.GetService<SomeDecorator>();
            var decorator2 = services.GetService<AnotherDecorator>();
            var sut = services.GetService<SomeClass>();

            // Act
            sut.MultiAttributeMethod();

            // Assert
            Assert.Equal(1, decorator1.OnAfterCallCount);
            Assert.Equal(1, decorator1.OnBeforeCallCount);
            Assert.Equal(1, decorator2.OnAfterCallCount);
            Assert.Equal(1, decorator2.OnBeforeCallCount);
        }

        [Fact]
        public void Method_WithSameDecoratorTwice_ShouldCallDecorMethodOnce()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<SomeDecorator>();
            var sut = services.GetService<ISomeInterface>();

            // Act
            sut.AttributeInInterfaceAndClassMethod();

            // Assert
            Assert.Equal(1, decorator.OnAfterCallCount);
            Assert.Equal(1, decorator.OnBeforeCallCount);
        }

        [Fact]
        public void Method_WithGenericParamters_ShouldCallDecorMethod()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<SomeDecorator>();
            var sut = services.GetService<ISomeInterface>();

            // Act
            sut.AttributeInClassWithReturnMethod(0);

            // Assert
            Assert.Equal(1, decorator.OnAfterCallCount);
            Assert.Equal(1, decorator.OnBeforeCallCount);
        }

        [Theory, AutoData]
        public void Method_WithReturn_ShouldReturnTheSame(int expectedReturnValue)
        {
            // Arrange
            var services = GetServices();
            var sut = services.GetService<ISomeInterface>();

            // Act
            var actualReturnValue = sut.AttributeInClassWithReturnMethod(expectedReturnValue);

            // Assert
            Assert.Equal(expectedReturnValue, actualReturnValue);
        }

        private IServiceProvider GetServices()
            => new ServiceCollection()
                .AddDecor()
                .AddSingleton<SomeDecorator>()
                .AddSingleton<AnotherDecorator>()
                .AddTransientDecorated<SomeClass>()
                .AddScopedDecorated<ISomeInterface, SomeClass>()
                .BuildServiceProvider();
    }
}
