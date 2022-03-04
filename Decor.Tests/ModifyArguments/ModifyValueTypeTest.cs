using System;
using System.Collections.Generic;
using System.Text;


// TODO: https://github.com/lawrence-laz/Decor.NET/blob/master/Decor.UnitTests/ChangeArgumentTests.cs
// TODO: cover negative cases
namespace Decor.Tests.ModifyArguments
{
    class ModifyValueTypeTest
    {
        //[Theory, AutoData]
        //public void GenericMethod_WithDecorator_ShouldOverrideReturnValue(int expectedValue)
        //{
        //    // Arrange
        //    var services = GetServices();
        //    var decorator = services.GetService<TestDecorator>();
        //    decorator.ExpectedArgumentValue = expectedValue;
        //    var someClass = services.GetService<SomeClass>();

        //    // Act
        //    var actual = someClass.Method(0); // "0" will be overridden by the decorator.

        //    // Assert
        //    actual.Should().Be(expectedValue);
        //}

        //#region Setup
        //public class TestDecorator : IDecorator
        //{
        //    public int ExpectedArgumentValue { get; set; }

        //    public async Task OnInvoke(Call call)
        //    {
        //        call.Arguments[0] = ExpectedArgumentValue;
        //        await call.Next();
        //    }
        //}

        //public class SomeClass
        //{
        //    [Decorate(typeof(TestDecorator))]
        //    virtual public int Method(int value) => value;
        //}

        //private IServiceProvider GetServices()
        //    => new ServiceCollection()
        //        .AddDecor()
        //        .AddSingleton<TestDecorator>()
        //        .AddTransientDecorated<SomeClass>()
        //        .BuildServiceProvider();
        //#endregion
    }
}
