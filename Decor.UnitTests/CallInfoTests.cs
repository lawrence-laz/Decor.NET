using AutoFixture.Xunit2;
using Decor.UnitTests.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Decor.UnitTests
{
    public class CallInfoTests
    {
        [Theory, AutoData]
        public void Method_WithDecorator_ShouldGetFilledCallInfo(int expectedReturnValue)
        {
            // Arrange
            var services = GetServices();
            var decorator = services.GetService<TestDecorator>();
            var sut = services.GetService<TestClass>();

            var expectedArgumentType = expectedReturnValue.GetType();
            var expectedObject = sut.UnwrapProxy();
            var expectedMethodName = nameof(sut.Method);

            // Act
            sut.Method(expectedReturnValue);

            // Assert
            var actual = decorator.CallInfo;
            var actualCalls = actual.GetState<Counter>();

            Assert.NotNull(actual);
            Assert.Equal(2, actualCalls.Count);
            Assert.Equal(expectedMethodName, actual.Method.Name);
            Assert.Equal(expectedObject, actual.Object);
            Assert.Equal(expectedReturnValue, actual.ReturnValue);
            Assert.Equal(expectedReturnValue, (int)actual.Arguments[0]);
            Assert.Equal(expectedArgumentType, actual.GenericArguments[0]);
        }

        private IServiceProvider GetServices()
            => new ServiceCollection()
                .AddDecor()
                .AddSingleton<TestDecorator>()
                .AddTransientDecorated<TestClass>()
                .BuildServiceProvider();

        public class TestClass { [Decorate(typeof(TestDecorator))] public virtual T Method<T>(T value) => value; }

        public class TestDecorator : IDecorator
        {
            public CallInfo CallInfo { get; set; }

            public void OnBefore(CallInfo callInfo) => (CallInfo = callInfo).SetState(new Counter { Count = 1 });

            public void OnAfter(CallInfo callInfo) => callInfo.GetState<Counter>().Count++;
        }
    }
}
