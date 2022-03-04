// using FluentAssertions;
// using System.Threading.Tasks;
// using Xunit;


// // TODO: https://github.com/lawrence-laz/Decor.NET/blob/master/Decor.UnitTests/MethodTypeTests.cs
// // TODO: Rename to AsyncTaskTest, and then add AsynsVoidTest ?

// namespace Decor.Tests.MethodTypes
// {
//     public class AsyncTaskTest
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

//         public class AsyncWithoutReturnClass
//         {
//             [Decorate(typeof(TestDecorator))]
//             public virtual async Task Method() => await Task.Yield();
//         }

//         [Fact]
//         public async Task Decorate_WithPublicClass_ShouldDecorate()
//         {
//             // Arrange
//             var decorator = new TestDecorator();
//             var sut = new AsyncWithoutReturnClassDecorated(decorator);

//             // Act
//             await sut.Method();

//             // Assert
//             decorator.WasCalled.Should().BeTrue();
//         }
//     }
// }
