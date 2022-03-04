// using FluentAssertions;
// using System;
// using System.Linq;
// using System.Threading.Tasks;
// using Xunit;

// namespace Decor.Tests.Attributes
// {
//     [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
//     public class Expected1Attribute : Attribute
//     {
//     }

//     public class AttributesRemainTest
//     {
//         //[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
//         //sealed class Expected2Attribute : Attribute
//         //{
//         //}

//         public class TestDecorator : IDecorator
//         {
//             public bool WasCalled;

//             public async Task OnInvoke(Invocation invocation)
//             {
//                 WasCalled = true;
//                 await invocation.Next();
//             }
//         }

//         internal class AttributesRemainClass
//         {
//             [Decorate(typeof(TestDecorator)), Expected1]
//             //[Expected2]
//             public virtual void Method() { }
//         }

//         [Fact]
//         public void Decorate_WithAdditionalAttributes_ShouldRemoveOnlyDecorateAttribute()
//         {
//             // Arrange
//             var decorator = new TestDecorator();
//             var sut = new AttributesRemainClassDecorated(decorator);
//             var actual = typeof(AttributesRemainClassDecorated)
//                 .GetMethod(nameof(AttributesRemainClass.Method))
//                 .GetCustomAttributes(false)
//                 .Select(x => x.GetType());

//             // Assert
//             actual.Should().BeEquivalentTo(typeof(Expected1Attribute)/*, typeof(Expected2Attribute)*/);
//         }
//     }
// }
