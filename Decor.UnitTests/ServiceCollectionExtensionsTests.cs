using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Decor.UnitTests
{
    public class ServiceCollectionExtensionsTests
    {
        #region Setup
        public class TestDecorator : IDecorator
        {
            public int CalledCount { get; private set; }
            public bool Called => CalledCount != 0;

            public Task OnInvoke(Call call)
            {
                CalledCount++;
                call.ReturnValue = CalledCount;
                return Task.CompletedTask;
            }
        }

        public class Test
        {
            [Decorate(typeof(TestDecorator))]
            public virtual int Method() { return 0; }
        }

        public class TestChild : Test { }

        public class NonDecorated1 { }

        public class NonDecorated2 { }

        #endregion

        [Fact]
        public void Decorate_WithSingleServiceTypeInstance_ShouldDecorateInstance()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
                .AddSingleton<Test>()
                .AddSingleton<TestDecorator>()
                .Decorate<Test>()
                .BuildServiceProvider();

            var decorator = serviceProvider.GetRequiredService<TestDecorator>();
            var decorated = serviceProvider.GetRequiredService<Test>();

            // Act
            decorated.Method();

            // Assert
            decorator.Called.Should().BeTrue();
        }

        [Fact]
        public void Decorate_WithMultipleServiceTypeInstances_ShouldDecorateAllInstances()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
                .AddSingleton<Test>()
                .AddSingleton<Test, TestChild>()
                .AddTransient<TestDecorator>()
                .Decorate<Test>() // Expecting this will decorate both singletons above, since they share service type.
                .BuildServiceProvider();

            var decoratedInstances = serviceProvider.GetServices<Test>().ToArray();

            // Act
            var actualDecoratorCallCount1 = decoratedInstances[0].Method();
            var actualDecoratorCallCount2 = decoratedInstances[1].Method();

            // Assert
            actualDecoratorCallCount1.Should().Be(1);
            actualDecoratorCallCount2.Should().Be(1); // Even though they share service type, decorator instances are not shared.
        }

        [Fact]
        public void Decorate_WithMultipleServices_OnlyDecoratedIsDecorated()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
                .AddSingleton<NonDecorated1>()
                .AddSingleton<Test>()
                .AddSingleton<NonDecorated2>()
                .AddSingleton<TestDecorator>()
                .Decorate<Test>()
                .BuildServiceProvider();

            // Act
            var decorated = serviceProvider.GetRequiredService<Test>();
            var nonDecorated1 = serviceProvider.GetRequiredService<NonDecorated1>();
            var nonDecorated2 = serviceProvider.GetRequiredService<NonDecorated2>();

            // Assert
            nonDecorated1.Should().BeOfType<NonDecorated1>();
            nonDecorated2.Should().BeOfType<NonDecorated2>();
            decorated.Should().NotBeOfType<Test>();
        }

        [Fact]
        public void Decorate_WithoutService_ThrowsArgumentException()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert
            services.Invoking(x => x.Decorate<Test>())
                .Should().Throw<ArgumentException>("because there is no service to decorate in service collection.");
        }
    }
}
