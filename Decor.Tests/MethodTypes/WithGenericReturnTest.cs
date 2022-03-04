// using AutoFixture.Xunit2;
// using FluentAssertions;
// using System.Threading.Tasks;
// using Xunit;

// namespace Decor.Tests.MethodTypes
// {
//     // TODO
//     public class WithGenericReturnTest
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

//         public class WithGenericReturnClass
//         {
//             [Decorate(typeof(TestDecorator))]
//             public virtual T Method<T>(T expected) => expected;
//         }

//         [Theory, AutoData]
//         public void Decorate_WithPublicClass_ShouldDecorate(object expected)
//         {
//             // Arrange
//             var decorator = new TestDecorator();
//             var sut = new WithGenericReturnClassDecorated(decorator);

//             // Act
//             var actual = sut.Method(expected);

//             // Assert
//             decorator.WasCalled.Should().BeTrue();
//             actual.Should().Be(expected);
//         }
//     }
// }
