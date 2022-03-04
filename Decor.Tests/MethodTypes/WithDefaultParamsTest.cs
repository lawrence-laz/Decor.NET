// using AutoFixture.Xunit2;
// using FluentAssertions;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Linq.Expressions;
// using System.Text;
// using System.Threading.Tasks;
// using Xunit;

// namespace Decor.Tests.MethodTypes
// {
//     public class WithDefaultParamsTest
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

//         public class WithDefaultParams
//         {
//             [Decorate(typeof(TestDecorator))]
//             public virtual int Method(int expected = 42) => expected;
//         }

//         [Theory, AutoData]
//         public void Decorate_WithDefaultParams_ShouldDecorate(int expected)
//         {
//             // Arrange
//             var decorator = new TestDecorator();
//             var sut = new WithDefaultParamsDecorated(decorator);

//             // Act
//             var actual = sut.Method(expected);

//             // Assert
//             decorator.WasCalled.Should().BeTrue();
//             actual.Should().Be(expected);
//         }
//     }
// }
