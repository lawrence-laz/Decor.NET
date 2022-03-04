// using AutoFixture.Xunit2;
// using FluentAssertions;
// using System.Threading.Tasks;
// using Xunit;

// namespace Decor.Tests.MethodTypes
// {
//     public class AsyncTaskWithReturnTest
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

//         public class AsyncWithReturnClass
//         {
//             [Decorate(typeof(TestDecorator))]
//             public virtual async Task<int> Method(int expected) => await Task.FromResult(expected);
//         }

//         [Theory, AutoData]
//         public async Task Decorate_WithPublicClass_ShouldDecorate(int expected)
//         {
//             // Arrange
//             var decorator = new TestDecorator();
//             var sut = new AsyncWithReturnClassDecorated(decorator);

//             // Act
//             var actual = await sut.Method(expected);

//             // Assert
//             decorator.WasCalled.Should().BeTrue();
//             actual.Should().Be(expected);
//         }
//     }
// }
