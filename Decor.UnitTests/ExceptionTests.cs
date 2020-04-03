using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Decor.UnitTests
{
    public class ExceptionTests
    {
        [Fact]
        public void Method_WithThrowException_ShouldCatchSameException()
        {
            // Arrange
            var services = GetServices();
            var someClass = services.GetService<SomeClass>();

            // Act & Assert
            someClass.Invoking(x => x.MethodThrow())
                .Should().ThrowExactly<ExpectedException>();
        }

        [Fact]
        public void MethodAsync_WithThrowImmediately_ShouldCatchSameException()
        {
            // Arrange
            var services = GetServices();
            var someClass = services.GetService<SomeClass>();

            // Act & Assert
            someClass.Invoking(async x => await x.MethodAsyncThrowImmediately())
                .Should().ThrowExactly<ExpectedException>();
        }

        [Fact]
        public void MethodAsyncResult_WithThrowImmediately_ShouldCatchSameException()
        {
            // Arrange
            var services = GetServices();
            var someClass = services.GetService<SomeClass>();

            // Act & Assert
            someClass.Invoking(async x => await x.MethodAsyncResultThrowImmediately())
                .Should().ThrowExactly<ExpectedException>();
        }

        [Fact]
        public void MethodAsync_WithThrowAfterAwait_ShouldCatchSameException()
        {
            // Arrange
            var services = GetServices();
            var someClass = services.GetService<SomeClass>();

            // Act & Assert
            someClass.Invoking(async x => await x.MethodAsyncThrowAfterAwait())
                .Should().ThrowExactly<ExpectedException>();
        }

        [Fact]
        public void MethodAsyncResult_WithThrowAfterAwait_ShouldCatchSameException()
        {
            // Arrange
            var services = GetServices();
            var someClass = services.GetService<SomeClass>();

            // Act & Assert
            someClass.Invoking(async x => await x.MethodAsyncResultThrowAfterAwait())
                .Should().ThrowExactly<ExpectedException>();
        }

        [Fact]
        public void Method_WithThrowExceptionNotDecorated_ShouldCatchSameException()
        {
            // Arrange
            var services = GetServices();
            var someClass = services.GetService<SomeClass>();

            // Act & Assert
            someClass.Invoking(x => x.MethodThrowNotDecorated())
                .Should().ThrowExactly<ExpectedException>();
        }

        [Fact]
        public void MethodAsync_WithThrowAfterAwaitNotDecorated_ShouldCatchSameException()
        {
            // Arrange
            var services = GetServices();
            var someClass = services.GetService<SomeClass>();

            // Act & Assert
            someClass.Invoking(async x => await x.MethodAsyncThrowAfterAwaitNotDecorated())
                .Should().ThrowExactly<ExpectedException>();
        }

        [Fact]
        public void MethodAsync_WithThrowImmediatelyNotDecorated_ShouldCatchSameException()
        {
            // Arrange
            var services = GetServices();
            var someClass = services.GetService<SomeClass>();

            // Act & Assert
            someClass.Invoking(async x => await x.MethodAsyncThrowImmediatelyNotDecorated())
                .Should().ThrowExactly<ExpectedException>();
        }

        [Fact]
        public void MethodAsyncResult_WithThrowAfterAwaitNotDecorated_ShouldCatchSameException()
        {
            // Arrange
            var services = GetServices();
            var someClass = services.GetService<SomeClass>();

            // Act & Assert
            someClass.Invoking(async x => await x.MethodAsyncResultThrowAfterAwaitNotDecorated())
                .Should().ThrowExactly<ExpectedException>();
        }

        [Fact]
        public void MethodAsyncResult_WithThrowImmediatelyNotDecorated_ShouldCatchSameException()
        {
            // Arrange
            var services = GetServices();
            var someClass = services.GetService<SomeClass>();

            // Act & Assert
            someClass.Invoking(async x => await x.MethodAsyncResultThrowImmediatelyNotDecorated())
                .Should().ThrowExactly<ExpectedException>();
        }

        [Fact]
        public void MethodDecoratedToThrow_WithThrowingDecorator_ShouldCatchSameException()
        {
            // Arrange
            var services = GetServices();
            var someClass = services.GetService<SomeClass>();

            // Act & Assert
            someClass.Invoking(x => x.MethodDecoratedToThrow())
                .Should().ThrowExactly<ExpectedException>();
        }

        [Fact]
        public void MethodDecoratedToThrowAsync_WithThrowingDecorator_ShouldCatchSameException()
        {
            // Arrange
            var services = GetServices();
            var someClass = services.GetService<SomeClass>();

            // Act & Assert
            someClass.Invoking(async x => await x.MethodDecoratedToThrowAsync())
                .Should().ThrowExactly<ExpectedException>();
        }

        [Fact]
        public void MethodDecoratedToThrowAsyncResult_WithThrowingDecorator_ShouldCatchSameException()
        {
            // Arrange
            var services = GetServices();
            var someClass = services.GetService<SomeClass>();

            // Act & Assert
            someClass.Invoking(async x => await x.MethodDecoratedToThrowAsyncResult())
                .Should().ThrowExactly<ExpectedException>();
        }

        #region Setup
        public class TestDecorator : IDecorator
        {
            public async Task OnInvoke(Call call)
            {
                await call.Next();
            }
        }

        public class ThrowingDecorator : IDecorator
        {
            public async Task OnInvoke(Call call) => throw new ExpectedException();
        }

        public class ExpectedException : Exception { }

        public class SomeClass
        {
            public int CallCount { get; set; }

            [Decorate(typeof(TestDecorator))]
            public virtual void MethodThrow() => throw new ExpectedException();

            [Decorate(typeof(TestDecorator))]
            public virtual Task MethodAsyncThrowImmediately() => throw new ExpectedException();

            [Decorate(typeof(TestDecorator))]
            public virtual async Task MethodAsyncThrowAfterAwait()
            {
                await Task.Yield();
                throw new ExpectedException();
            }

            [Decorate(typeof(TestDecorator))]
            public virtual Task<int> MethodAsyncResultThrowImmediately() => throw new ExpectedException();

            [Decorate(typeof(TestDecorator))]
            public virtual async Task<int> MethodAsyncResultThrowAfterAwait()
            {
                await Task.Yield();
                throw new ExpectedException();
            }

            public virtual void MethodThrowNotDecorated() => throw new ExpectedException();

            public virtual Task MethodAsyncThrowImmediatelyNotDecorated() => throw new ExpectedException();

            public virtual async Task MethodAsyncThrowAfterAwaitNotDecorated()
            {
                await Task.Yield();
                throw new ExpectedException();
            }

            public virtual Task<int> MethodAsyncResultThrowImmediatelyNotDecorated() => throw new ExpectedException();

            public virtual async Task<int> MethodAsyncResultThrowAfterAwaitNotDecorated()
            {
                await Task.Yield();
                throw new ExpectedException();
            }

            [Decorate(typeof(ThrowingDecorator))]
            public virtual void MethodDecoratedToThrow() { }

            [Decorate(typeof(ThrowingDecorator))]
            public virtual async Task MethodDecoratedToThrowAsync() => await Task.Yield();

            [Decorate(typeof(ThrowingDecorator))]
            public virtual async Task<int> MethodDecoratedToThrowAsyncResult() => await Task.FromResult(0);
        }

        private IServiceProvider GetServices()
            => new ServiceCollection()
                .AddDecor()
                .AddSingleton<TestDecorator>()
                .AddSingleton<ThrowingDecorator>()
                .AddTransient<SomeClass>().Decorated()
                .BuildServiceProvider();
        #endregion
    }
}
