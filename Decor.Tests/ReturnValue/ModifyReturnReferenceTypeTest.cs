// using AutoFixture.Xunit2;
// using FluentAssertions;
// using System.Threading.Tasks;
// using Xunit;

// namespace Decor.Tests.ReturnValue
// {

//     // TODO: https://github.com/lawrence-laz/Decor.NET/blob/master/Decor.UnitTests/ChangeReturnValueTests.cs
//     // TODO: https://github.com/lawrence-laz/Decor.NET/blob/master/Decor.UnitTests/ReturnValueTests.cs
//     public class ModifyReturnReferenceTypeTest
//     {
//         public class TestDecorator : IDecorator
//         {
//             public object ExpectedValue;

//             public async Task OnInvoke(Invocation invocation)
//             {
//                 await invocation.Next();
//                 invocation.ReturnValue = ExpectedValue;
//             }
//         }

//         public class ReferenceTypeReturningClass
//         {
//             [Decorate(typeof(TestDecorator))]
//             public virtual object Method() => new object();
//         }

//         [Theory, AutoData]
//         public void ModifyReturnValue_WithReferenceTypeReturn_ShouldReturnModified(int expected)
//         {
//             // Arrange
//             var decorator = new TestDecorator
//             {
//                 ExpectedValue = expected
//             };
//             var sut = new ReferenceTypeReturningClassDecorated(decorator);

//             // Act
//             var actual = sut.Method();

//             // Assert
//             actual.Should().Be(expected);
//         }
//     }
// }
