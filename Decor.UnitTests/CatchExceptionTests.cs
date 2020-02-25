using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;


namespace Decor.UnitTests
{
    public class CatchExceptionTests
    {
        [Fact]
        public void Method_WithExceptionCatchingDecorator_ShouldCatchException()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            var someClass = services.GetService<SomeClass>();

            // Act
            someClass.ThrowingMethod();

            // Assert
            decorator.Exception.Should().NotBeNull();
        }

        [Fact]
        public async Task MethodAsync_WithExceptionCatchingDecorator_ShouldCatchException()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            var someClass = services.GetService<SomeClass>();

            // Act
            await someClass.ThrowingMethodAsync();

            // Assert
            decorator.Exception.Should().NotBeNull();
        }

        #region Setup
        public class TestDecorator : IDecorator
        {
            public Exception Exception { get; set; }

            public async Task OnInvoke(Call call)
            {
                try
                {
                    await call.Next();
                }
                catch (Exception e)
                {
                    Exception = e;
                }
            }
        }

        public class SomeClass
        {
            [Decorate(typeof(TestDecorator))]
            virtual public void ThrowingMethod() => throw new Exception();
            [Decorate(typeof(TestDecorator))]
            virtual public async Task ThrowingMethodAsync() { await Task.Delay(100); throw new Exception(); }
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
