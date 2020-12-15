using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace Decor.UnitTests
{
    public class DependencyLifeTimeTests
    {
        [Fact]
        public void I_can_decorate_with_decorator_that_has_scoped_dependency()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
                .AddScoped<Dependency>()
                .AddTransient<TestDecoratorWithDependency>()
                .AddScoped<ISomeInterface, SomeClass>()
                .Decorate<ISomeInterface>()
                .BuildServiceProvider();

            // Act & Assert
            serviceProvider.GetRequiredService<ISomeInterface>();
        }

        #region Setup
        public class Dependency { }

        public class TestDecoratorWithDependency : IDecorator
        {
            public TestDecoratorWithDependency(Dependency dependency)
            {
                Dependency = dependency;
            }

            public Dependency Dependency { get; }

            public Task OnInvoke(Call call) => Task.CompletedTask;
        }

        public interface ISomeInterface
        {
            void SomeMethod();
        }

        public class SomeClass : ISomeInterface
        {
            [Decorate(typeof(TestDecoratorWithDependency))]
            public void SomeMethod() { }
        }
        #endregion
    }
}
