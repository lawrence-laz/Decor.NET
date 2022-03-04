using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Decor.UnitTests
{
    public class DynamicTypeTests
    {
        [Fact]
        public async Task SomeMethod_WithDynamicSut_ShouldCallDecorator()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            dynamic sut = services.GetService<ISomeInterface>();

            // Act
            await sut.SomeMethod();

            // Assert
            decorator.CallCountBefore.Should().Be(1);
            decorator.CallCountAfter.Should().Be(1);
        }

        [Theory, AutoData]
        public async Task SomeMethod_WithDynamicSutAndParameter_ShouldCallDecorator(int expected)
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            dynamic sut = services.GetService<ISomeInterface>();

            // Act
            var actual = await sut.SomeMethodWithParameterAndResult((dynamic)expected);

            // Assert
            decorator.CallCountBefore.Should().Be(1);
            decorator.CallCountAfter.Should().Be(1);
            ((int)actual).Should().Be(expected);
        }

        #region Setup
        public class TestDecorator : IDecorator
        {
            public int CallCountBefore { get; set; }
            public int CallCountAfter { get; set; }

            public async Task OnInvoke(Call call)
            {
                CallCountBefore++;
                await call.Next();
                CallCountAfter++;
            }
        }

        public interface ISomeInterface
        {
            Task SomeMethod();
            Task<int> SomeMethodWithParameterAndResult(int argument);
        }

        public class SomeClass : ISomeInterface
        {
            [Decorate(typeof(TestDecorator))]
            public async Task SomeMethod() { await Task.Yield(); }

            [Decorate(typeof(TestDecorator))]
            public async Task<int> SomeMethodWithParameterAndResult(int argument)
            {
                await Task.Yield();
                return argument;
            }
        }

        private IServiceProvider GetServices()
            => new ServiceCollection()
                .AddDecor()
                .AddSingleton<TestDecorator>()
                .AddTransient<ISomeInterface, SomeClass>().Decorated()
                .BuildServiceProvider();
        #endregion
    }
}
