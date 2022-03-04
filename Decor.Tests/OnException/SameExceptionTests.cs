// using System;
// using System.Threading.Tasks;
// using Xunit;
// using FluentAssertions;

// namespace Decor.Tests.OnException
// {
//     public class SameExceptionTests
//     {
//         [Fact]
//         public void Method_DecoratedAndThrows_ShouldCatchSameException()
//         {
//             // Arrange
//             var sut = new TestClassDecorated(new TestDecorator(), new ThrowingDecorator());

//             // Act & Assert
//             sut.Invoking(x => x.MethodThrow())
//                 .Should().ThrowExactly<ExpectedException>();
//         }

//         [Fact]
//         public void MethodAsync_DecoratedAndThrowsImmediately_ShouldCatchSameException()
//         {
//             // Arrange
//             var sut = new TestClassDecorated(new TestDecorator(), new ThrowingDecorator());

//             // Act & Assert
//             sut.Invoking(x => x.MethodAsyncThrowImmediately())
//                 .Should().ThrowExactly<ExpectedException>();
//         }

//         [Fact]
//         public void MethodAsync_DecoratedAndThrowsAfterAwait_ShouldCatchSameException()
//         {
//             // Arrange
//             var sut = new TestClassDecorated(new TestDecorator(), new ThrowingDecorator());

//             // Act & Assert
//             sut.Invoking(x => x.MethodAsyncThrowAfterAwait())
//                 .Should().ThrowExactly<ExpectedException>();
//         }

//         [Fact]
//         public void MethodAsync_WithResultDecoratedAndThrowsImmediately_ShouldCatchSameException()
//         {
//             // Arrange
//             var sut = new TestClassDecorated(new TestDecorator(), new ThrowingDecorator());

//             // Act & Assert
//             sut.Invoking(x => x.MethodAsyncResultThrowImmediately())
//                 .Should().ThrowExactly<ExpectedException>();
//         }

//         [Fact]
//         public void MethodAsync_WithResultDecoratedAndThrowsAfterAwait_ShouldCatchSameException()
//         {
//             // Arrange
//             var sut = new TestClassDecorated(new TestDecorator(), new ThrowingDecorator());

//             // Act & Assert
//             sut.Invoking(x => x.MethodAsyncResultThrowAfterAwait())
//                 .Should().ThrowExactly<ExpectedException>();
//         }

//         [Fact]
//         public void Method_NotDecoratedAndThrows_ShouldCatchSameException()
//         {
//             // Arrange
//             var sut = new TestClassDecorated(new TestDecorator(), new ThrowingDecorator());

//             // Act & Assert
//             sut.Invoking(x => x.MethodThrowNotDecorated())
//                 .Should().ThrowExactly<ExpectedException>();
//         }

//         [Fact]
//         public void MethodAsync_NotDecoratedAndThrowsImmediately_ShouldCatchSameException()
//         {
//             // Arrange
//             var sut = new TestClassDecorated(new TestDecorator(), new ThrowingDecorator());

//             // Act & Assert
//             sut.Invoking(x => x.MethodAsyncThrowImmediatelyNotDecorated())
//                 .Should().ThrowExactly<ExpectedException>();
//         }

//         [Fact]
//         public void MethodAsync_NotDecoratedAndThrowsAfterAwait_ShouldCatchSameException()
//         {
//             // Arrange
//             var sut = new TestClassDecorated(new TestDecorator(), new ThrowingDecorator());

//             // Act & Assert
//             sut.Invoking(x => x.MethodAsyncThrowAfterAwaitNotDecorated())
//                 .Should().ThrowExactly<ExpectedException>();
//         }

//         [Fact]
//         public void MethodAsync_WithResultNotDecoratedAndThrowsImmediately_ShouldCatchSameException()
//         {
//             // Arrange
//             var sut = new TestClassDecorated(new TestDecorator(), new ThrowingDecorator());

//             // Act & Assert
//             sut.Invoking(x => x.MethodAsyncResultThrowImmediatelyNotDecorated())
//                 .Should().ThrowExactly<ExpectedException>();
//         }

//         [Fact]
//         public void MethodAsync_WithResultNotDecoratedAndThrowsAfterAwait_ShouldCatchSameException()
//         {
//             // Arrange
//             var sut = new TestClassDecorated(new TestDecorator(), new ThrowingDecorator());

//             // Act & Assert
//             sut.Invoking(x => x.MethodAsyncResultThrowAfterAwaitNotDecorated())
//                 .Should().ThrowExactly<ExpectedException>();
//         }

//         [Fact]
//         public void Method_DecoratedAndDecoratorThrows_ShouldCatchSameException()
//         {
//             // Arrange
//             var sut = new TestClassDecorated(new TestDecorator(), new ThrowingDecorator());

//             // Act & Assert
//             sut.Invoking(x => x.MethodDecoratedToThrow())
//                 .Should().ThrowExactly<ExpectedException>();
//         }

//         [Fact]
//         public void MethodAsync_DecoratedAndDecoratorThrows_ShouldCatchSameException()
//         {
//             // Arrange
//             var sut = new TestClassDecorated(new TestDecorator(), new ThrowingDecorator());

//             // Act & Assert
//             sut.Invoking(x => x.MethodDecoratedToThrowAsync())
//                 .Should().ThrowExactly<ExpectedException>();
//         }

//         [Fact]
//         public void MethodAsync_WithResultDecoratedAndDecoratorThrows_ShouldCatchSameException()
//         {
//             // Arrange
//             var sut = new TestClassDecorated(new TestDecorator(), new ThrowingDecorator());

//             // Act & Assert
//             sut.Invoking(x => x.MethodDecoratedToThrowAsyncResult())
//                 .Should().ThrowExactly<ExpectedException>();
//         }

//         #region Setup
//         public class TestDecorator : IDecorator
//         {
//             public async Task OnInvoke(Invocation invocation)
//             {
//                 await invocation.Next();
//             }
//         }

//         public class ThrowingDecorator : IDecorator
//         {
//             public async Task OnInvoke(Invocation invocation)
//             {
//                 await invocation.Next();
//                 throw new ExpectedException();
//             }
//         }

//         public class ExpectedException : Exception { }

//         public class TestClass
//         {
//             [Decorate(typeof(TestDecorator))]
//             public virtual void MethodThrow() => throw new ExpectedException();

//             [Decorate(typeof(TestDecorator))]
//             public virtual Task MethodAsyncThrowImmediately() => throw new ExpectedException();

//             [Decorate(typeof(TestDecorator))]
//             public virtual async Task MethodAsyncThrowAfterAwait()
//             {
//                 await Task.Yield();
//                 throw new ExpectedException();
//             }

//             [Decorate(typeof(TestDecorator))]
//             public virtual Task<int> MethodAsyncResultThrowImmediately() => throw new ExpectedException();

//             [Decorate(typeof(TestDecorator))]
//             public virtual async Task<int> MethodAsyncResultThrowAfterAwait()
//             {
//                 await Task.Yield();
//                 throw new ExpectedException();
//             }

//             public virtual void MethodThrowNotDecorated() => throw new ExpectedException();

//             public virtual Task MethodAsyncThrowImmediatelyNotDecorated() => throw new ExpectedException();

//             public virtual async Task MethodAsyncThrowAfterAwaitNotDecorated()
//             {
//                 await Task.Yield();
//                 throw new ExpectedException();
//             }

//             public virtual Task<int> MethodAsyncResultThrowImmediatelyNotDecorated() => throw new ExpectedException();

//             public virtual async Task<int> MethodAsyncResultThrowAfterAwaitNotDecorated()
//             {
//                 await Task.Yield();
//                 throw new ExpectedException();
//             }

//             [Decorate(typeof(ThrowingDecorator))]
//             public virtual void MethodDecoratedToThrow() { }

//             [Decorate(typeof(ThrowingDecorator))]
//             public virtual async Task MethodDecoratedToThrowAsync() => await Task.Yield();

//             [Decorate(typeof(ThrowingDecorator))]
//             public virtual async Task<int> MethodDecoratedToThrowAsyncResult() => await Task.FromResult(0);
//         }
//         #endregion
//     }
// }
