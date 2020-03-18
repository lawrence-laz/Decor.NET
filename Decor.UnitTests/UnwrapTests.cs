using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Decor.UnitTests
{
    public class UnwrapTests
    {
        [Fact]
        public void UnwrapDecorate_WithDecoratedObject_ShouldReturnUnderlyingObject()
        {
            // Arrange
            var services = GetServices();
            var sut = services.GetService<ISomeInterface>();

            // Act
            var actual = sut.UnwrapDecorated();

            // Assert
            actual.GetType().Should().Be(typeof(SomeDecoratedClass));
        }

        [Fact]
        public void UnwrapDecorate_WithNotDecoratedObject_ShouldReturnSameObject()
        {
            // Arrange
            var expected = new SomeNotDecoratedClass();

            // Act
            var actual = expected.UnwrapDecorated();

            // Assert
            actual.Should().Be(expected);
        }

        [Theory, AutoData]
        public void SomeMethod_WithUnwrappedDecorated_ShouldNotCallDecorator(int expected)
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            var sut = services.GetService<ISomeInterface>().UnwrapDecorated();

            // Act
            var actual = sut.SomeMethod(expected);

            // Assert
            actual.Should().Be(expected);
            decorator.CallBeforeCount.Should().Be(0);
            decorator.CallAfterCount.Should().Be(0);
        }

        #region Setup
        public class TestDecorator : IDecorator
        {
            public int CallBeforeCount { get; set; }
            public int CallAfterCount { get; set; }

            public async Task OnInvoke(Call call)
            {
                CallBeforeCount++;
                await call.Next();
                CallAfterCount++;
            }
        }

        public interface ISomeInterface
        {
            T SomeMethod<T>(T argument);
        }

        public class SomeDecoratedClass : ISomeInterface
        {
            [Decorate(typeof(TestDecorator))]
            public T SomeMethod<T>(T argument) => argument;
        }

        public class SomeNotDecoratedClass { }

        private IServiceProvider GetServices()
            => new ServiceCollection()
                .AddDecor()
                .AddSingleton<TestDecorator>()
                .AddTransient<ISomeInterface, SomeDecoratedClass>().Decorated()
                .BuildServiceProvider();
        #endregion
    }
}
