using Decor.UnitTests.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace Decor.UnitTests
{
    public class PerformanceTests
    {
        public ITestOutputHelper Output { get; }

        public PerformanceTests(ITestOutputHelper output)
        {
            Output = output;
        }

        [Fact(DisplayName = "<300 ms on first call.")]
        public void Method_CalledFirstTime_ShouldTakeLessThan300Ms()
        {
            // Arrange
            var services = GetServices();
            services.GetService<SomeDecorator>(); // Warm up ServiceProvider
            var stopwatch = Stopwatch.StartNew();

            // Act
            services.GetService<ISomeInterface>().AttributeInClassMethod();

            // Assert
            var actualTime = stopwatch.ElapsedMilliseconds;
            Output.WriteLine($"Initial call took '{actualTime}' ms.");
            Assert.True(actualTime < 300, $"Initial call to a decorated class took {actualTime} ms.");
        }

        [Fact(DisplayName = "<5 ms after first call.")]
        public void Method_CalledAfterTheFirstTime_ShouldTakeLessThan5Ms()
        {
            // Arrange
            var services = GetServices();
            services.GetService<ISomeInterface>().AttributeInClassMethod();
            var stopwatch = Stopwatch.StartNew();

            // Act
            services.GetService<ISomeInterface>().AttributeInClassMethod();

            // Assert
            var actualTime = stopwatch.ElapsedMilliseconds;
            Output.WriteLine($"Subsequent call took '{actualTime}' ms.");
            Assert.True(actualTime < 5, $"Subsequent call to a decorated class took {actualTime} ms.");
        }

        private IServiceProvider GetServices()
            => new ServiceCollection()
                .AddDecor()
                .AddTransient<SomeAsyncDecorator>()
                .AddTransient<SomeDecorator>()
                .AddTransient<AnotherDecorator>()
                .AddTransient<DecoratorWithDependencies>()
                .AddTransientDecorated<SomeClass>()
                .AddTransientDecorated<ISomeInterface, SomeClass>()
                .AddTransient<SomeDependency>()
                .BuildServiceProvider();
    }
}
