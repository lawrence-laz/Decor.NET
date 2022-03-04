using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Decor.UnitTests
{
    public class AttributeLocationTests
    {
        [Fact]
        public void Method_DecoratedInInterface_ShouldCallDecoratorOnce()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            var decoratedClass = services.GetService<ISomeInterface>();

            // Act
            decoratedClass.DecoratedInInterface();

            // Assert
            decorator.CallCountBefore.Should().Be(1);
            decorator.CallCountAfter.Should().Be(1);
        }

        [Fact]
        public void Method_DecoratedInClass_ShouldCallDecoratorOnce()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            var decoratedClass = services.GetService<ISomeInterface>();

            // Act
            decoratedClass.DecoratedInClass();

            // Assert
            decorator.CallCountBefore.Should().Be(1);
            decorator.CallCountAfter.Should().Be(1);
        }

        [Fact]
        public void Method_DecoratedInInterfaceAndClassWithSame_ShouldCallDecoratorOnce()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            var decoratedClass = services.GetService<ISomeInterface>();

            // Act
            decoratedClass.DecoratedInInterfaceAndClassWithSame();

            // Assert
            decorator.CallCountBefore.Should().Be(1);
            decorator.CallCountAfter.Should().Be(1);
        }

        [Fact]
        public void Method_DecoratedInInterfaceAndClassWithDifferent_ShouldCallEachDecoratorOnce()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            var anotherDecorator = services.GetService<AnotherTestDecorator>();
            var decoratedClass = services.GetService<ISomeInterface>();

            // Act
            decoratedClass.DecoratedInInterfaceAndClassWithDifferent();

            // Assert
            decorator.CallCountBefore.Should().Be(1);
            decorator.CallCountAfter.Should().Be(1);
            anotherDecorator.CallCountBefore.Should().Be(1);
            anotherDecorator.CallCountAfter.Should().Be(1);
        }

        [Fact]
        public void Method_WithNoAttribute_ShouldNotCallDecorMethods()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            var anotherDecorator = services.GetService<AnotherTestDecorator>();
            var sut = services.GetService<ISomeInterface>();

            // Act
            sut.NotDecoratedMethod();

            // Assert
            decorator.CallCountBefore.Should().Be(0);
            decorator.CallCountAfter.Should().Be(0);
            anotherDecorator.CallCountBefore.Should().Be(0);
            anotherDecorator.CallCountAfter.Should().Be(0);
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

        public class AnotherTestDecorator : IDecorator
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
            [Decorate(typeof(TestDecorator))]
            void DecoratedInInterface();
            
            void DecoratedInClass();

            [Decorate(typeof(TestDecorator))]
            void DecoratedInInterfaceAndClassWithSame();

            [Decorate(typeof(TestDecorator))]
            void DecoratedInInterfaceAndClassWithDifferent();
            void NotDecoratedMethod();
        }

        public class SomeClass : ISomeInterface
        {
            public void DecoratedInInterface() { }

            [Decorate(typeof(TestDecorator))]
            public void DecoratedInClass() { }

            [Decorate(typeof(TestDecorator))]
            public void DecoratedInInterfaceAndClassWithSame() { }

            [Decorate(typeof(AnotherTestDecorator))]
            public void DecoratedInInterfaceAndClassWithDifferent() { }

            public void NotDecoratedMethod() { }
        }

        private IServiceProvider GetServices()
            => new ServiceCollection()
                .AddDecor()
                .AddSingleton<TestDecorator>()
                .AddSingleton<AnotherTestDecorator>()
                .AddTransientDecorated<ISomeInterface, SomeClass>()
                .BuildServiceProvider();
        #endregion
    }
}
