// using AutoFixture.Xunit2;
// using FluentAssertions;
// using System.Threading.Tasks;
// using Xunit;

// namespace Decor.Tests.MethodTypes
// {
//     public class WithReturnTest
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

//         public class WithReturnClass
//         {
//             [Decorate(typeof(TestDecorator))]
//             public virtual int Method(int expected) => expected;
//         }

//         [Theory, AutoData]
//         public void Decorate_WithPublicClass_ShouldDecorate(int expected)
//         {
//             // Arrange
//             var decorator = new TestDecorator();
//             var sut = new WithReturnClassDecorated(decorator);

//             // Act
//             var actual = sut.Method(expected);

//             // Assert
//             decorator.WasCalled.Should().BeTrue();
//             actual.Should().Be(expected);
//         }
//     }
// }
