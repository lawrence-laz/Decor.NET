using Castle.DynamicProxy;
using System.Collections.Generic;
using System.Reflection;

namespace Decor.Internal
{
    public class AttributeInterceptor : ProcessingAsyncInterceptor<object>
    {
        private readonly IDecorator _decorator;
        private readonly ISet<MethodInfo> _targetMethods;

        public AttributeInterceptor(IDecorator decorator, ISet<MethodInfo> targetMethods)
        {
            _decorator = decorator;
            _targetMethods = targetMethods;
        }

        protected override object StartingInvocation(IInvocation invocation)
        {
            if (IsInTargetMethods(invocation.MethodInvocationTarget, invocation.Method))
            {
                var callInfo = new CallInfo(invocation);
                _decorator.OnBefore(callInfo);
            }

            return null;
        }

        protected override void CompletedInvocation(IInvocation invocation, object state)
        {
            if (IsInTargetMethods(invocation.MethodInvocationTarget, invocation.Method))
            {
                var callInfo = new CallInfo(invocation);
                _decorator.OnAfter(callInfo);
            }
        }

        private bool IsInTargetMethods(params MethodInfo[] methods)
        {
            // TODO: Test which is more performant--going through the custom attributes or checking if
            // an ISet<MethodInfo> contains this method.

            foreach (var method in methods)
            {
                if (_targetMethods.Contains(method))
                {
                    return true;
                }

                if (method.IsGenericMethod && !method.IsGenericMethodDefinition
                    && _targetMethods.Contains(method.GetGenericMethodDefinition()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
