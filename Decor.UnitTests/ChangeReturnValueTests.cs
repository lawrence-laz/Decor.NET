using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Decor.UnitTests
{
    public class ChangeReturnValueTests
    {
        [Theory, AutoData]
        public void SyncMethod_WithDecorator_ShouldOverrideReturnValue(int expectedValue)
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            decorator.ExpectedReturnValue = expectedValue;
            var someClass = services.GetService<SomeClass>();

            // Act
            var actual = someClass.SyncMethod();

            // Assert
            actual.Should().Be(expectedValue);
        }

        [Theory, AutoData]
        public async Task AsyncMethod_WithDecorator_ShouldOverrideReturnValue(int expectedValue)
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            decorator.ExpectedReturnValue = expectedValue;
            var someClass = services.GetService<SomeClass>();

            // Act
            var actual = await someClass.AsyncMethod();

            // Assert
            actual.Should().Be(expectedValue);
        }

        [Theory, AutoData]
        public void GenericMethod_WithDecorator_ShouldOverrideReturnValue(int expectedValue)
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            decorator.ExpectedReturnValue = expectedValue;
            var someClass = services.GetService<SomeClass>();

            // Act
            var actual = someClass.GenericMethod<int>();

            // Assert
            actual.Should().Be(expectedValue);
        }

        #region Setup
        public class TestDecorator : IDecorator
        {
            public int ExpectedReturnValue { get; set; }

            public async Task OnInvoke(Call call)
            {
                await call.Next();
                call.ReturnValue = ExpectedReturnValue;
            }
        }

        public class SomeClass
        {
            [Decorate(typeof(TestDecorator))]
            virtual public int SyncMethod() => default;
            [Decorate(typeof(TestDecorator))]
            virtual public async Task<int> AsyncMethod() { await Task.Delay(100); return default; }
            [Decorate(typeof(TestDecorator))]
            virtual public T GenericMethod<T>() => default;
        }

        private IServiceProvider GetServices()
            => new ServiceCollection()
                .AddDecor()
                .AddSingleton<TestDecorator>()
                .AddTransientDecorated<SomeClass>()
                .BuildServiceProvider();
        #endregion
    }
}
