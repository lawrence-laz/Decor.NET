// using AutoFixture.Xunit2;
// using FluentAssertions;
// using System.Threading.Tasks;
// using Xunit;

// namespace Decor.Tests.MethodTypes
// {
//     public class ExpressionBodyWithReturnTest
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

//         public class ExpressionBodyWithReturnClass
//         {
//             [Decorate(typeof(TestDecorator))]
//             public virtual object Method(object input) => input;
//         }

//         [Theory, InlineAutoData]
//         public void Decorate_WithPublicClass_ShouldDecorate(object expected)
//         {
//             // Arrange
//             var decorator = new TestDecorator();
//             var sut = new ExpressionBodyWithReturnClassDecorated(decorator);

//             // Act
//             var actual = sut.Method(expected);

//             // Assert
//             actual.Should().Be(expected);
//             decorator.WasCalled.Should().BeTrue();
//         }
//     }
// }
