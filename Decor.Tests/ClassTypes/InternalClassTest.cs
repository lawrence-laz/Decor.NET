// using FluentAssertions;
// using System.Threading.Tasks;
// using Xunit;

// namespace Decor.Tests.ClassTypes
// {
//     public class InternalClassTest
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

//         internal class InternalClass
//         {
//             [Decorate(typeof(TestDecorator))]
//             public virtual void Method() { }
//         }

//         [Fact]
//         public void Decorate_WithPublicClass_ShouldDecorate()
//         {
//             // Arrange
//             var decorator = new TestDecorator();
//             var sut = new InternalClassDecorated(decorator);

//             // Act
//             sut.Method();

//             // Assert
//             decorator.WasCalled.Should().BeTrue();
//         }
//     }
// }
