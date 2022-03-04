using System;
using System.Collections.Generic;
using System.Text;

namespace Decor.Tests.Performance
{
    // TODO: https://github.com/lawrence-laz/Decor.NET/blob/master/Decor.UnitTests/PerformanceTests.cs
    class PerformanceTests
    {
        //public ITestOutputHelper Output { get; }

        //public PerformanceTests(ITestOutputHelper output)
        //{
        //    Output = output;
        //}

        ///// <summary>
        ///// .Net Framework 4.6.1 seems to be a bit slower and needs ~600ms, while .Net Core fits in ~200 ms.
        ///// </summary>
        //[Fact(Skip = "Should run on-demand.", DisplayName = "<600 ms on first call.")]
        //public void Method_CalledFirstTime_ShouldTakeLessThanSpecifiedDuration()
        //{
        //    // Arrange
        //    var services = GetServices();
        //    services.GetService<TestDecorator>(); // Warm up ServiceProvider
        //    var stopwatch = Stopwatch.StartNew();

        //    // Act
        //    services.GetService<ISomeInterface>().Method();

        //    // Assert
        //    var actualTime = stopwatch.ElapsedMilliseconds;
        //    Output.WriteLine($"Initial call took '{actualTime}' ms.");
        //    actualTime.Should().BeLessThan(600);
        //}

        //[Fact(Skip = "Should run on-demand.", DisplayName = "<=2 ms after first call.")]
        //public void Method_CalledAfterTheFirstTime_ShouldTakeLessThanSpecifiedDuration()
        //{
        //    // Arrange
        //    var services = GetServices();
        //    services.GetService<ISomeInterface>().Method(); // Warm up dynamic proxy
        //    var stopwatch = Stopwatch.StartNew();

        //    // Act
        //    services.GetService<ISomeInterface>().Method();

        //    // Assert
        //    var actualTime = stopwatch.ElapsedMilliseconds;
        //    Output.WriteLine($"Subsequent call took '{actualTime}' ms.");
        //    actualTime.Should().BeLessThan(2);
        //}

        //#region Setup
        //public class TestDecorator : IDecorator
        //{
        //    public int CallCount { get; set; }

        //    public async Task OnInvoke(Call call)
        //    {
        //        CallCount++;
        //        await call.Next();
        //    }
        //}

        //public interface ISomeInterface
        //{
        //    [Decorate(typeof(TestDecorator))]
        //    void Method();
        //}

        //public class SomeClass : ISomeInterface
        //{
        //    public void Method() { }
        //}

        //private IServiceProvider GetServices()
        //    => new ServiceCollection()
        //        .AddDecor()
        //        .AddSingleton<TestDecorator>()
        //        .AddTransient<ISomeInterface, SomeClass>().Decorated()
        //        .BuildServiceProvider();
        //#endregion
    }
}
