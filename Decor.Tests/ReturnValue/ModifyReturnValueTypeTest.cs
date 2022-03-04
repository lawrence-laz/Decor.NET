// using AutoFixture.Xunit2;
// using FluentAssertions;
// using System;
// using System.Threading.Tasks;
// using Xunit;

// // TODO: Handle cases when classes are defined within other classes and that causes collisions in root namespace (multiple TestClass).

// namespace Decor.Tests.ReturnValue
// {
//     public class ModifyReturnValueTypeTest
//     {
//         public class TestDecorator : IDecorator
//         {
//             public int ExpectedValue;

//             public async Task OnInvoke(Invocation invocation)
//             {
//                 await invocation.Next();
//                 invocation.ReturnValue = ExpectedValue;
//             }
//         }

//         public class ValueTypeReturningClass
//         {
//             [Decorate(typeof(TestDecorator))]
//             public virtual int Method() => new Random().Next();
//         }

//         [Theory, AutoData]
//         public void ModifyReturnValue_WithValueTypeReturn_ShouldReturnModified(int expected)
//         {
//             // Arrange
//             var decorator = new TestDecorator
//             {
//                 ExpectedValue = expected
//             };
//             var sut = new ValueTypeReturningClassDecorated(decorator);

//             // Act
//             var actual = sut.Method();

//             // Assert
//             actual.Should().Be(expected);
//         }
//     }
// }
