// using FluentAssertions;
// using System;
// using System.Threading.Tasks;
// using Xunit;

// namespace Decor.Tests.MethodTypes
// {
//     public class ExpressionBodyVoidTest
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

//         public class ExpressionBodyVoidClass
//         {
//             [Decorate(typeof(TestDecorator))]
//             public virtual void Method() => Console.Write("Hello, world!");
//         }

//         [Fact]
//         public void Decorate_WithPublicClass_ShouldDecorate()
//         {
//             // Arrange
//             var decorator = new TestDecorator();
//             var sut = new ExpressionBodyVoidClassDecorated(decorator);

//             // Act
//             sut.Method();

//             // Assert
//             decorator.WasCalled.Should().BeTrue();
//         }
//     }
// }
