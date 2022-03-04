using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Decor.UnitTests
{
    public class ChangeArgumentTests
    {
        [Theory, AutoData]
        public void GenericMethod_WithDecorator_ShouldOverrideReturnValue(int expectedValue)
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            decorator.ExpectedArgumentValue = expectedValue;
            var someClass = services.GetService<SomeClass>();

            // Act
            var actual = someClass.Method(0); // "0" will be overridden by the decorator.

            // Assert
            actual.Should().Be(expectedValue);
        }

        #region Setup
        public class TestDecorator : IDecorator
        {
            public int ExpectedArgumentValue { get; set; }

            public async Task OnInvoke(Call call)
            {
                call.Arguments[0] = ExpectedArgumentValue;
                await call.Next();
            }
        }

        public class SomeClass
        {
            [Decorate(typeof(TestDecorator))]
            virtual public int Method(int value) => value;
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
