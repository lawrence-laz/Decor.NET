using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Decor.UnitTests
{
    public class RegistrationTests
    {
        [Fact]
        public void Decorate_TypeDescriptor_ShouldBeDecorated()
        {
            // Arrange
            var services = GetTypeServices();
            var decorator = services.GetService<TestDecorator>();
            var someClass = services.GetService<SomeClass>();

            // Act
            someClass.Method();

            // Assert
            decorator.CallCount.Should().Be(1);
        }
        
        [Fact]
        public void Decorate_InstanceDescriptor_ShouldBeDecorated()
        {
            // Arrange
            var services = GetInstanceServices();
            var decorator = services.GetService<TestDecorator>();
            var someClass = services.GetService<SomeClass>();

            // Act
            someClass.Method();

            // Assert
            decorator.CallCount.Should().Be(1);
        }
        
        [Fact]
        public void Decorate_FactoryDescriptor_ShouldBeDecorated()
        {
            // Arrange
            var services = GetFactoryServices();
            var decorator = services.GetService<TestDecorator>();
            var someClass = services.GetService<SomeClass>();

            // Act
            someClass.Method();

            // Assert
            decorator.CallCount.Should().Be(1);
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
            void Method();
        }

        public class SomeClass : ISomeInterface
        {
            [Decorate(typeof(TestDecorator))]
            virtual public void Method() { }
        }

        private IServiceProvider GetTypeServices()
            => new ServiceCollection()
                .AddDecor()
                .AddSingleton<TestDecorator>()
                .AddTransient<ISomeInterface, SomeClass>().Decorated()
                .AddTransient<SomeClass>().Decorated()
                .BuildServiceProvider();

        private IServiceProvider GetInstanceServices()
        {
            var instance = new SomeClass();

            return new ServiceCollection()
                .AddDecor()
                .AddSingleton<TestDecorator>()
                .AddSingleton<ISomeInterface>(instance).Decorated()
                .AddSingleton(instance).Decorated()
                .BuildServiceProvider();
        }

        private IServiceProvider GetFactoryServices()
        {
            return new ServiceCollection()
                .AddDecor()
                .AddSingleton<TestDecorator>()
                .AddTransient<ISomeInterface>(_ => new SomeClass()).Decorated()
                .AddTransient(_ => new SomeClass()).Decorated()
                .BuildServiceProvider();
        }
        #endregion
    }
}
