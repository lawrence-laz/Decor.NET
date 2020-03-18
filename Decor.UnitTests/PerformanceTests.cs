using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
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

        /// <summary>
        /// .Net Framework 4.6.1 seems to be a bit slower and needs ~600ms, while .Net Core fits in ~200 ms.
        /// </summary>
        [Fact(DisplayName = "<600 ms on first call.")]
        public void Method_CalledFirstTime_ShouldTakeLessThan300Ms()
        {
            // Arrange
            var services = GetServices();
            services.GetService<TestDecorator>(); // Warm up ServiceProvider
            var stopwatch = Stopwatch.StartNew();

            // Act
            services.GetService<ISomeInterface>().Method();

            // Assert
            var actualTime = stopwatch.ElapsedMilliseconds;
            Output.WriteLine($"Initial call took '{actualTime}' ms.");
            actualTime.Should().BeLessThan(600);
        }

        [Fact(DisplayName = "<=1 ms after first call.")]
        public void Method_CalledAfterTheFirstTime_ShouldTakeLessThan5Ms()
        {
            // Arrange
            var services = GetServices();
            services.GetService<ISomeInterface>().Method(); // Warm up dynamic proxy
            var stopwatch = Stopwatch.StartNew();

            // Act
            services.GetService<ISomeInterface>().Method();

            // Assert
            var actualTime = stopwatch.ElapsedMilliseconds;
            Output.WriteLine($"Subsequent call took '{actualTime}' ms.");
            actualTime.Should().BeLessThan(2);
        }

        #region Setup
        public class TestDecorator : IDecorator
        {
            public int CallCount { get; set; }

            public async Task OnInvoke(Call call)
            {
                CallCount++;
                await call.Next();
            }
        }

        public interface ISomeInterface
        {
            [Decorate(typeof(TestDecorator))]
            void Method();
        }

        public class SomeClass : ISomeInterface
        {
            public void Method() { }
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
