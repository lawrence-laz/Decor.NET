using AutoFixture.Xunit2;
using Decor.UnitTests.Utils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Decor.UnitTests
{
    public class CallTests
    {
        [Theory, AutoData]
        public void GenericInterfaceMethod_WithDecorator_ShouldHaveFilledCallObject(int expectedReturnValue)
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            var someClass = services.GetService<ISomeInterface>();

            // Act
            someClass.GenericMethod(expectedReturnValue);

            // Assert
            decorator.Call.Should().NotBeNull();

            decorator.Call.Method.GetGenericMethodDefinition()
                .Should().BeSameAs(typeof(ISomeInterface).GetMethod(nameof(ISomeInterface.GenericMethod)));
            
            decorator.Call.MethodImplementation.GetGenericMethodDefinition()
                .Should().BeSameAs(typeof(SomeClass).GetMethod(nameof(SomeClass.GenericMethod)));
            
            decorator.Call.Object.Should().Be(someClass.UnwrapProxy());
            
            decorator.Call.ReturnValue.Should().Be(expectedReturnValue);
            
            ((int)decorator.Call.Arguments[0]).Should().Be(expectedReturnValue);
            
            decorator.Call.GenericArguments[0].Should().Be(expectedReturnValue.GetType());
        }

        [Fact]
        public void NonVirtualMethod_WithDecorator_ShouldHaveMethodImplementationSameAsMethod()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            var someClass = services.GetService<SomeClass>();

            // Act
            someClass.Method();

            // Assert
            decorator.Call.Method.Should().BeSameAs(decorator.Call.MethodImplementation);
        }

        [Fact]
        public void NonGenericMethod_WithDecorator_ShouldHaveGenericArgumentsEmpty()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            var someClass = services.GetService<SomeClass>();

            // Act
            someClass.Method();

            // Assert
            decorator.Call.GenericArguments.Should().BeEmpty();
        }

        #region Setup
        public class TestDecorator : IDecorator
        {
            public Call Call { get; set; }

            public async Task OnInvoke(Call call)
            {
                Call = call;
                await call.Next();
            }
        }

        public interface ISomeInterface
        {
            T GenericMethod<T>(T arg);
        }

        public class SomeClass : ISomeInterface
        {
            [Decorate(typeof(TestDecorator))]
            public T GenericMethod<T>(T arg) => arg;

            [Decorate(typeof(TestDecorator))]
            virtual public void Method() { }
        }

        private IServiceProvider GetServices()
            => new ServiceCollection()
                .AddDecor()
                .AddSingleton<TestDecorator>()
                .AddTransientDecorated<ISomeInterface, SomeClass>()
                .AddTransientDecorated<SomeClass>()
                .BuildServiceProvider();
        #endregion
    }
}
