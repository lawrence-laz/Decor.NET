using AutoFixture.Xunit2;
using Castle.DynamicProxy;
using Decor.UnitTests.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Xunit;

namespace Decor.UnitTests
{
    public class CallInfoTests
    {
        [Theory, AutoData]
        public void Method_WithBeforeAndAfterDecorators_ShouldGetCallInfo(int expectedReturnValue)
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<SomeDecorator>();
            var sut = services.GetService<ISomeInterface>();
            var expectedType = expectedReturnValue.GetType();
            var expectedMethodName = nameof(sut.AttributeInClassWithReturnMethod);

            // Act
            var actualReturnValue = sut.AttributeInClassWithReturnMethod(expectedReturnValue);

            // Assert
            var actualBefore = decorator.CallInfoBefore;
            var actualAfter = decorator.CallInfoAfter;
            var expectedObject = UnwrapProxy(sut);

            Assert.NotNull(actualBefore);
            Assert.NotNull(actualAfter);

            Assert.Equal(expectedMethodName, actualBefore.Method.Name);
            Assert.Equal(expectedMethodName, actualAfter.Method.Name);

            Assert.Equal(expectedObject, actualBefore.Object);
            Assert.Equal(expectedObject, actualAfter.Object);

            Assert.Equal(expectedReturnValue, actualBefore.ReturnValue);
            Assert.Equal(expectedReturnValue, actualAfter.ReturnValue);

            Assert.Equal(expectedReturnValue, (int)actualBefore.Arguments[0]);
            Assert.Equal(expectedReturnValue, (int)actualAfter.Arguments[0]);

            Assert.Equal(expectedType, actualBefore.GenericArguments[0]);
            Assert.Equal(expectedType, actualAfter.GenericArguments[0]);
        }

        private T UnwrapProxy<T>(T proxy)
            => ProxyUtil.IsProxy(proxy)
                ? (T)proxy.GetType()
                    .GetField("__target", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(proxy)
                : proxy;

        private IServiceProvider GetServices()
            => new ServiceCollection()
                .AddDecor()
                .AddSingleton<SomeAsyncDecorator>()
                .AddSingleton<SomeDecorator>()
                .AddSingleton<AnotherDecorator>()
                .AddSingleton<DecoratorWithDependencies>()
                .AddTransientDecorated<SomeClass>()
                .AddScopedDecorated<ISomeInterface, SomeClass>()
                .AddTransient<SomeDependency>()
                .BuildServiceProvider();
    }
}
