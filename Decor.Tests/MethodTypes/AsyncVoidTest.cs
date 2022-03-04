// using FluentAssertions;
// using System.Threading.Tasks;
// using Xunit;

// namespace Decor.Tests.MethodTypes
// {
//     public class AsyncVoidTest
//     {
//         public class TestDecorator : IDecorator
//         {
//             public bool WasCalled;

//             public async Task OnInvoke(Invocation invocation)
//             {
//                 WasCalled = true;
//                 await invocation.Next();
//             }
//         }

//         public class AsyncVoidClass
//         {
//             [Decorate(typeof(TestDecorator))]
//             public virtual async void Method() => await Task.Yield();
//         }

//         [Fact]
//         public async Task DecorateMethod_AsyncVoid_ShouldCallDecorator()
//         {
//             // Arrange
//             var decorator = new TestDecorator();
//             var sut = new AsyncVoidClassDecorated(decorator);

//             // Act
//             sut.Method();

//             // Assert
//             decorator.WasCalled.Should().BeTrue();
//         }
//     }
// }
