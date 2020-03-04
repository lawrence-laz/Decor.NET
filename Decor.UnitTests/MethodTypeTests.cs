using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Decor.UnitTests
{
    public class MethodTypeTests
    {
        [Fact]
        public void SyncMethod_WithOneAttributeInClass_ShouldCallDecoratorOnce()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            var decoratedClass = services.GetService<SomeClass>();

            // Act
            decoratedClass.Method();

            // Assert
            decorator.CallCountBefore.Should().Be(1);
            decorator.CallCountAfter.Should().Be(1);
        }

        [Fact]
        public async Task AsyncMethod_WithOneAttributeInClass_ShouldCallDecoratorOnce()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            var decoratedClass = services.GetService<SomeClass>();

            // Act
            await decoratedClass.AsyncMethod();

            // Assert
            decorator.CallCountBefore.Should().Be(1);
            decorator.CallCountAfter.Should().Be(1);
        }

        [Fact]
        public async Task AsyncMethodWithResult_WithOneAttributeInClass_ShouldCallDecoratorOnce()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            var decoratedClass = services.GetService<SomeClass>();

            // Act
            await decoratedClass.AsyncMethodWithResult();

            // Assert
            decorator.CallCountBefore.Should().Be(1);
            decorator.CallCountAfter.Should().Be(1);
        }

        [Fact]
        public void GenericMethod_WithOneAttributeInClass_ShouldCallDecoratorOnce()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            var sut = services.GetService<SomeClass>();

            // Act
            sut.GenericMethod<int>();

            // Assert
            decorator.CallCountBefore.Should().Be(1);
            decorator.CallCountAfter.Should().Be(1);
        }


        [Fact]
        public async Task AsyncMethod_WithoutAwaits_ShouldCallDecoratorOnce()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            var sut = services.GetService<SomeClass>();

            // Act
            await sut.AsyncMethod(shouldAwait: false);

            // Assert
            decorator.CallCountBefore.Should().Be(1);
            decorator.CallCountAfter.Should().Be(1);
        }

        [Fact]
        public async Task AsyncMethodWithResult_WithoutAwaits_ShouldCallDecoratorOnce()
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            var sut = services.GetService<SomeClass>();

            // Act
            await sut.AsyncMethodWithResult(shouldAwait: false);

            // Assert
            decorator.CallCountBefore.Should().Be(1);
            decorator.CallCountAfter.Should().Be(1);
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

        public class SomeClass
        {
            [Decorate(typeof(TestDecorator))]
            virtual public void Method() { }
            [Decorate(typeof(TestDecorator))]
            virtual public async Task AsyncMethod() { await Task.Delay(100); }
            [Decorate(typeof(TestDecorator))]
            virtual public async Task<object> AsyncMethodWithResult() { await Task.Delay(100); return null; }
            [Decorate(typeof(TestDecorator))]
            virtual public T GenericMethod<T>() => default;
            [Decorate(typeof(TestDecorator))]
            virtual public async Task AsyncMethod(bool shouldAwait)
            {
                if (shouldAwait) await Task.Delay(100);
            }
            [Decorate(typeof(TestDecorator))]
            virtual public async Task<object> AsyncMethodWithResult(bool shouldAwait)
            {
                if (shouldAwait) await Task.Delay(100);

                return new object();
            }
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
