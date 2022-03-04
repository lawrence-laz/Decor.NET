using System.Xml.Xsl.Runtime;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using System;

namespace Decor.Tests.ClassTypes
{
    public class PublicClassTest
    {
        public class TestDecorator : IDecorator
        {
            public bool WasCalled;

            public async Task OnInvoke(Invocation invocation)
            {
                WasCalled = true;
                await invocation.Next();
            }
        }

        public class PublicClass
        {
            [Decorate(typeof(TestDecorator))]
            public virtual void Method() { }

            public virtual void Method2(string aa, int bb) { }
            public virtual void Method2(string aa, string bb) { }
        }

        public static class Decorate
        {
            public static void Class<T>() where T : class
            {

            }
        }

        [Fact]
        public void Decorate_WithPublicClass_ShouldDecorate()
        {
            // // Arrange
            // var decorator = new TestDecorator();
            // var sut = new PublicClassDecorated(decorator);

            // // Act
            // sut.Method();

            // // Assert
            // decorator.WasCalled.Should().BeTrue();
        }

        [Fact]
        public void Foo()
        {
            typeof(PublicClassTest)
                .Assembly
                .GetTypes()
                .Should()
                .Contain(type => type.Name.EndsWith("DecorMarker"));

            DecorMarker.Hello.Should().Be("Hello");

            Decorateee
                // .Types(type => type.IsAssignableFrom(typeof(IInterface)))
                .Type<PublicClass>()   // PublicClassDecorated
                .Method(x => x.Method2)
                // .Method()
                // .Method(x => x.Method())
                // .Method(x => x.Method(It.Any<int>())) // 😥
                // .Method(x => x.Method) // 😀
                // .Method<int>(x => x.Method) // 😀
                // .Method(x => (Action<int>)x.Method) // 😐️
                .With<TestDecorator>();
                // .As<PublicClass2>();
                // .With(async (invocation) => { await invocation.Next(); }) // Inline decorator, invocation.Context for services
                // .As<PublicClassDecorated>() // Specify custom name for the decorated class.
                // .As(type => type.Name.FullName + "Decorated") // Selector for multi type decorations

            Decorateee
                // .Types(type => type is ICommandHandler) // Expression lambda selector
                .Type<PublicClass>()
                .With<TestDecorator>();
                // .Methods(method => method.IsPublic) // Expression lambda selector
                // .Method("MethodName") // Ambiguity with overloads? Register all by name?


                Decorate.Class<PublicClass>();
        }
    }



    // public interface IFoo
    // {
    //     public void Bar();
    // }

    // public class Foo : IFoo
    // {
    //     public virtual void Bar() 
    //     {
    //         //SomeDecorator

    //         // body

    //         // SomeDecorator
    //     }
    // }

    // public class SomeDecorator : IDecorator
    // {
    //     public void Inercept(Invocation invocation)
    //     {
    //         ///...
    //         invocation.Next();
    //         /// ..
    //     }
    // }

    // Decorate
    //     .Type<Foo>()
    //     .Method(x => Bar)
    //     .With<SomeDecorator>();

    // public class FooDecorated : IFoo
    // {
    //     private Foo foo = new();
    //     private IDecorator[] _bar_decorators = new[] { new SomeDecorator() };

    //     public void Bar() 
    //     {
    //         _bar_decorators.invoke() -> foo.Bar();
    //     }
    // }

    //     public class FooDecorated : Foo
    // {
    //     private IDecorator[] _bar_decorators = new[] { new SomeDecorator() };

    //     public void Bar() 
    //     {
    //         _bar_decorators.invoke() -> foo.Bar();
    //     }
    // }
}
