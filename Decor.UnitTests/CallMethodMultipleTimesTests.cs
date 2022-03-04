using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Decor.UnitTests
{
    public class CallMethodMultipleTimesTests
    {
        [Fact]
        public void Method_WithRetryDecorator_ShouldCallTargetTwice()
        {
            // Arrange
            var services = GetServices();
            var someClass = services.GetService<SomeClass>();
            var followingDecorator = services.GetService<CountingDecorator>();

            // Act
            someClass.Method();

            // Assert
            someClass.UnwrapDecorated().CallCount.Should().Be(2);
            followingDecorator.CallCount.Should().Be(2);
        }

        #region Setup
        public class TestDecorator : IDecorator
        {
            public async Task OnInvoke(Call call)
            {
                try
                {
                    await call.Next();
                }
                catch (Exception)
                {
                    try
                    {
                        await call.Next();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public class CountingDecorator : IDecorator
        {
            public int CallCount { get; set; }

            public async Task OnInvoke(Call call)
            {
                CallCount++;
                await call.Next();
            }
        }

        public class SomeClass
        {
            public int CallCount { get; set; }

            [Decorate(typeof(TestDecorator))]
            [Decorate(typeof(CountingDecorator))]
            public virtual void Method()
            {
                CallCount++;
                throw new Exception();
            }
        }

        private IServiceProvider GetServices()
            => new ServiceCollection()
                .AddDecor()
                .AddSingleton<TestDecorator>()
                .AddSingleton<CountingDecorator>()
                .AddTransient<SomeClass>().Decorated()
                .BuildServiceProvider();
        #endregion
    }
}
